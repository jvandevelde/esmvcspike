using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nest;
using Nest.Resolvers.Converters;
using SkillsEsSpike;
using SQLite;
using WebApplication1.App_Data;
using WebApplication1.App_Data.@extern.models;

namespace WebApplication1.Controllers
{
    public class ResultModel
    {
        
        public ResultModel()
        {
            Results = new SearchResponse<EmployeeSkillsDocument>();
            InputQuery = "_all:*";
        }

        public static ResultModel Default
        {
            get { return new ResultModel(); }
        }

        public int CurrentPage { get; set; }

        public long TotalPages
        {
            get { return Results.Total/25; }
        }

        public ISearchResponse<EmployeeSkillsDocument> Results { get; set; }
        public string InputQuery { get; set; }
    }
    
    public class HomeController : Controller
    {
        private const string DefaultEsEndpoint = "http://192.168.99.100:9200";
        private const string EsIndexName = "skills-spike";
        private const int PAGE_SIZE = 25;
        private ElasticClient _client;

        public HomeController()
        {
            var esEndpoint = ConfigurationManager.AppSettings["Elasticsearch.Server.Endpoint"]
                ?? DefaultEsEndpoint;
            //txtElasticsearchServerUri.Text = esEndpoint;

            var settings = new ConnectionSettings(new Uri(esEndpoint), EsIndexName);
            _client = new ElasticClient(settings);
        }
        
        public ActionResult Index()
        {
            
            return View(ResultModel.Default);
        }

        [HttpPost]
        public ActionResult Index(string query)
        {
            var results = Search(query);

            return View(results);
        }

        private ResultModel Search(string query, int page = 0)
        {
            var resp = _client.Search<EmployeeSkillsDocument>(
                d => d
                    .From(PAGE_SIZE * page)
                    .Take(PAGE_SIZE)
                    .QueryString(query)
                    .Highlight(h =>
                        h.OnFields(
                            f => f.OnField(ef => ef.Skills),
                            f => f.OnField(ef => ef.Certifications))
                            .PreTags("<mark>")
                            .PostTags("</mark>"))
                    .Aggregations(a => a
                        .Terms("emp_skills_agg", t => t.Field(p => p.Skills))
                        .Terms("emp_cert_agg", t => t.Field(p => p.Certifications))
                    )
                );

            var results = new ResultModel
            {
                Results = resp,
                InputQuery = query,
                CurrentPage = page
            };
            return results;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GenerateData()
        {
            _client.DeleteIndex(EsIndexName);
            var path = HttpContext.Server.MapPath("~/App_Data/names.db");
            var db = new SQLiteConnection(path);
            var female = db.Table<female>().ToList();
            var surname = db.Table<surname>().ToList();

            var rng = new Random();

            const int numEmployeesToCreate = 300;
            var numSkillsGenerated = 0;
            var numCertsGenerated = 0;
            for (var i = 0; i <= numEmployeesToCreate; i++)
            {
                var emp = new EmployeeSkillsDocument()
                {
                    Id = i,
                    FirstName = female[rng.Next(0, female.Count - 1)].name,
                    LastName = surname[rng.Next(0, surname.Count - 1)].name,
                    Skills = new List<string>(),
                    Certifications = new List<string>()
                };

                for (var j = 0; j <= 10; j++)
                {
                    emp.Skills.Add(SampleDatasets.Skills[rng.Next(0, SampleDatasets.Skills.Count - 1)]);
                    numSkillsGenerated++;
                }

                var numCerts = rng.Next(1, 10);
                for (var k = 0; k <= numCerts; k++)
                {
                    emp.Certifications.Add(SampleDatasets.Certifications[rng.Next(0, SampleDatasets.Certifications.Count - 1)]);
                    numCertsGenerated++;
                }

                _client.Index(emp);
            }

            return Content(string.Format("Created {0} employee records with {1} skills and {2} certs in index '{3}'",
                    numEmployeesToCreate, numSkillsGenerated, numCertsGenerated, EsIndexName));
        }


        public ActionResult AddQueryString(string query)
        {
            var results = Search(query);
            return View("Index", results);
        }

        public ActionResult NavigatePage(string query, int page)
        {
            var results = Search(query, page);
            return View("Index", results);
        }
    }
}
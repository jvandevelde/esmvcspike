using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Elasticsearch.Net;
using Nest;
using WebApplication1.Models;

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
            get { return Results.Total / 25; }
        }

        public ISearchResponse<EmployeeSkillsDocument> Results { get; set; }
        public string InputQuery { get; set; }
    }

    public class SearchController : Controller
    {
        private const string DefaultEsEndpoint = "http://192.168.99.100:9200";
        private const string EsIndexName = "skills-spike";
        private const int PageSize = 25;
        private readonly ElasticClient _client;

        public SearchController()
        {
            var esEndpoint = ConfigurationManager.AppSettings["Elasticsearch.Server.Endpoint"]
                ?? DefaultEsEndpoint;

            var settings = new ConnectionSettings(new Uri(esEndpoint), EsIndexName)
                .EnableTrace()
                .ExposeRawResponse();
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
                    .From(PageSize * page)
                    .Take(PageSize)
                    // Query string is being used to demo what can be done in ES easily.
                    // This is considered the 'Lite' version of search, but does not expose all the search
                    // capabilities of the system
                    // See https://www.elastic.co/guide/en/elasticsearch/guide/current/search-lite.html 
                    // vs https://www.elastic.co/guide/en/elasticsearch/guide/current/query-dsl-intro.html
                    // for more details
                    .QueryString(query)
                    .DefaultOperator(DefaultOperator.And)
                    .Highlight(h =>
                        h.OnFields(
                        f => f.OnAll().PreTags("<b>").PostTags("</b>"),
                            f => f.OnField(ef => ef.Skills.First().Name).PreTags("<mark-green>").PostTags("</mark-green>"),
                            f => f.OnField(ef => ef.Certifications.First().Name).PreTags("<mark-lyellow>").PostTags("</mark-lyellow>"))
                            )
                    .Aggregations(a => a
                        .Terms("emp_skills_agg", t => t.Field(p => p.Skills.First().Name))
                        .Terms("emp_cert_agg", t => t.Field(p => p.Certifications.First().Name))
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

        public ActionResult GenerateData()
        {
            if(_client.IndexExists(EsIndexName).Exists)
            {
                _client.DeleteIndex(EsIndexName);
            }

            ElasticsearchIndexManager.AddAutocompleteMappingToIndex(_client);
            ElasticsearchIndexManager.GenerateLookupDocumentLists(_client);
            var path = HttpContext.Server.MapPath("~/App_Data/names.db");
            var resultString = ElasticsearchIndexManager.GenerateRandomEmployeeDocuments(_client, path);

            return Content(resultString);
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
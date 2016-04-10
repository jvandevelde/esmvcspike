using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Nest;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class EmployeeController : Controller
    {
        private const string DefaultEsEndpoint = "http://192.168.99.100:9200";
        private const string EsIndexName = "skills-spike";
        private const int PageSize = 25;
        private readonly ElasticClient _client;

        public EmployeeController()
        {
            var esEndpoint = ConfigurationManager.AppSettings["Elasticsearch.Server.Endpoint"]
                ?? DefaultEsEndpoint;

            var settings = new ConnectionSettings(new Uri(esEndpoint), EsIndexName)
                .EnableTrace()
                .ExposeRawResponse();
            _client = new ElasticClient(settings);
        }
        
        public ActionResult Index(int employeeId)
        {
            var results = GetEmployee(employeeId);

            return View(results);
        }

        private EmployeeSkillsDocument GetEmployee(int employeeId)
        {
            var resp = _client.Get<EmployeeSkillsDocument>(emp => emp.Id(employeeId));
            
            if (!resp.Found)
                return new EmployeeSkillsDocument();

            return resp.Source;
        }

        public JsonResult AutoCompleteSkills(string query)
        {
            var searchResponse = _client.Search<Skill>(s => s
                 .Query(q => q
                     .Prefix("name.autocomplete", query.ToLower()))
                 .SortAscending(sort => sort.Name.Suffix("sortable")));
            
            return Json(searchResponse.Documents, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AutoCompleteCertifications(string query)
        {
            var searchResponse = _client.Search<Certification>(s => s
                 .Query(q => q
                     .Prefix("name.autocomplete", query.ToLower()))
                 .SortAscending(sort => sort.Name.Suffix("sortable")));

            return Json(searchResponse.Documents, JsonRequestBehavior.AllowGet);
        }
    }
}
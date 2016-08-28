using Microsoft.AspNet.Mvc;
using SkillsDatabase.Search.Api.Models;

namespace SkillsDatabase.Search.Api.Controllers
{
    [Route("api/[controller]")]
    public class AutocompleteController : Controller
    {
        [FromServices]
        public IElasticsearchClientProvider EsClientProvider { get; set; }

        private const int PageSize = 25;

        [HttpGet("skills/{prefix}")]
        public JsonResult AutoCompleteSkills(string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                return Json("Must specify query");

            var searchResponse = EsClientProvider.Instance.Search<EmployeeSkill>(s => s
                 .Query(q => q
                     .Prefix("name.autocomplete", prefix.ToLower()))
                     .Sort(sort => sort.Ascending(f => f.Name)));
            //.SortAscending(sort => sort.Name.Suffix("sortable")));

            return Json(searchResponse.Documents);
        }

        [HttpGet("certs/{prefix}")]
        public JsonResult AutoCompleteCertifications(string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                return Json("Must specify query");

            var searchResponse = EsClientProvider.Instance.Search<EmployeeCertification>(s => s
                 .Query(q => q
                     .Prefix("name.autocomplete", prefix.ToLower()))
                 .Sort(sort => sort.Ascending(f => f.Name)));
            //.SortAscending(sort => sort.Name.Suffix("sortable")));

            return Json(searchResponse.Documents);
        }
    }
}

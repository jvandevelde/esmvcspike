using Microsoft.AspNet.Mvc;
using SkillsDatabase.Search.Api.Models;
using SkillsDatabase.Search.Api;
using Nest;
using System.Web.Http;
using System.Linq;
using Microsoft.Extensions.PlatformAbstractions;

namespace SkillsDatabase.Search.Controllers
{
    [Route("api/[controller]")]
    public class SearchController : Controller
    {
        public IElasticsearchClientProvider EsClientProvider { get; set; }

        private const int PageSize = 25;

        public SearchController(IApplicationEnvironment env, IElasticsearchClientProvider esClientProvider)
        {
            EsClientProvider = esClientProvider;
        }

        // POST api/search
        [HttpGet]
        public ResultModel Post([FromUri]SearchRequestParameters parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters.Query))
                return new ResultModel();

            var resp = EsClientProvider.Instance.Search<EmployeeSkillsDocument>(
                d => d
                .From(PageSize * parameters.Page)
                .Take(PageSize)
                .Explain()
                .Query(q => q.QueryString(c =>

                    c.Name("named_query")
                    .Fields(f => f.Field(p => p.Certifications.First().Name)
                                .Field(p => p.Skills.First().Name))
                    .Query(parameters.Query)
                    
                    .FuzzyPrefixLength(2)
                    .FuzzyMaxExpansions(50)
                    .FuzzyRewrite(RewriteMultiTerm.ConstantScore)
                    .Rewrite(RewriteMultiTerm.ConstantScore)
                    .Fuziness(Fuzziness.EditDistance(3))
                ))
                    .Highlight(h =>
                        h.Fields(
                            f => f.Field(ef => ef.Skills.First().Name).PreTags("<mark-green>").PostTags("</mark-green>"),
                            f => f.Field(ef => ef.Certifications.First().Name).PreTags("<mark-lyellow>").PostTags("</mark-lyellow>"))
                            )
                    .Aggregations(a => a
                        .Terms("emp_skills_agg", t => t.Field(p => p.Skills.First().Name))
                        .Terms("emp_cert_agg", t => t.Field(p => p.Certifications.First().Name))
                    )
                );

            var results = new ResultModel(resp)
            {
                InputQuery = parameters.Query,
                CurrentPage = parameters.Page
            };

            return results;
        }
    }
}

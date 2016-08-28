using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillsDatabase.Search.Api.Models
{
    public class EmployeeSkillsDocument
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<EmployeeSkill> Skills { get; set; }
        public List<EmployeeCertification> Certifications { get; set; }
    }

    public class ResultModel
    {
        public ResultModel() { }
        public ResultModel(ISearchResponse<EmployeeSkillsDocument> searchResponse)
        {
            if (!searchResponse.IsValid)
                return;

            TotalDocs = searchResponse.Total;
            SkillsAggregates = searchResponse.Aggs.Terms("emp_skills_agg").Buckets.Select(x => new ResultAggregation
            {
                Key = x.Key,
                DocCount = x.DocCount
            }).ToList();
            CertificationAggregates = searchResponse.Aggs.Terms("emp_cert_agg").Buckets.Select(x => new ResultAggregation
            {
                Key = x.Key,
                DocCount = x.DocCount
            }).ToList();
            Hits = searchResponse.Hits.Select(x => new ResultHit<EmployeeSkillsDocument>()
            {
                Source = x.Source,
                HightlightHtmlList = x.Highlights.SelectMany(hl => hl.Value.Highlights).ToList()
            }).ToList();

            InputQuery = "_all:*";
        }

        public static ResultModel Default
        {
            get { return new ResultModel(); }
        }

        public int CurrentPage { get; set; }

        public long TotalPages
        {
            get { return TotalDocs / 25; }
        }


        public long TotalDocs { get; set; }
        public List<ResultAggregation> SkillsAggregates { get; set; }
        public List<ResultAggregation> CertificationAggregates { get; set; }
        public List<ResultHit<EmployeeSkillsDocument>> Hits { get; set; }
        public string InputQuery { get; set; }
    }

    public class ResultAggregation
    {
        public string Key { get; set; }
        public long? DocCount { get; set; }

    }

    public class ResultHit<T>
    {
        public T Source { get; set; }
        public List<string> HightlightHtmlList { get; set; }
    }
}

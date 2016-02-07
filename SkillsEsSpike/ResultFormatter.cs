using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nest;

namespace SkillsEsSpike
{
    class ResultFormatter
    {
        public static string FormatResults(ISearchResponse<EmployeeSkillsDocument> resp)
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("Found {0} documents. Showing first {1}", resp.Total, resp.Documents.Count()));
            foreach (var doc in resp.Documents)
            {
                sb.AppendLine(string.Format("{0} {1}", doc.FirstName, doc.LastName));

                foreach (var skill in doc.Skills)
                {
                    sb.AppendLine(string.Format("\t{0}", skill));
                }
            }

            return sb.ToString();
        }

        public static string FormatCommomSkills(ISearchResponse<EmployeeSkillsDocument> resp)
        {
            var sb = new StringBuilder();

            var popularSkills = resp.Aggs.Terms("emp_skills_agg");
            if (popularSkills != null && popularSkills.Items != null)
            {
                foreach (var skill in popularSkills.Items)
                {
                    sb.AppendLine(string.Format("{0} ({1}/{2} docs)", skill.Key, skill.DocCount, resp.Total));
                }
            }
            return sb.ToString();
        }
    }
}

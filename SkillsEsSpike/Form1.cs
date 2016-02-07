using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Nest;
using SkillsEsSpike.@extern.models;
using SQLite;

namespace SkillsEsSpike
{
    public partial class Form1 : Form
    {
        private const string DefaultEsEndpoint = "http://192.168.99.100:9200";
        private const string EsIndexName = "skills-spike";
        private readonly ElasticClient _client;

        public Form1()
        {
            InitializeComponent();
            
            var esEndpoint = ConfigurationManager.AppSettings["Elasticsearch.Server.Endpoint"]
                ?? DefaultEsEndpoint;
            txtElasticsearchServerUri.Text = esEndpoint;

            var settings = new ConnectionSettings(new Uri(esEndpoint), EsIndexName);
            _client = new ElasticClient(settings);
        }

        private void cmdGenerate_Click(object sender, EventArgs e)
        {
            //_client.DeleteByQuery<EmployeeSkillsDocument>(del => del
            //    .Query(q => q.QueryString(qs => qs.Query("*"))
            //    ));
            _client.DeleteIndex(EsIndexName);

            var db = new SQLiteConnection(@"extern/names.db");
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
                    FirstName = female[rng.Next(0, female.Count-1)].name,
                    LastName = surname[rng.Next(0, surname.Count - 1)].name,
                    Skills = new List<string>(),
                    Certifications = new List<string>()
                };
                
                for (var j = 0; j <= 10; j++)
                {
                    emp.Skills.Add(SampleDatasets.Skills[rng.Next(0, SampleDatasets.Skills.Count - 1)]);
                    numSkillsGenerated++;
                }

                var numCerts = rng.Next(1,10);
                for (var k = 0; k <= numCerts; k++)
                {
                    emp.Certifications.Add(SampleDatasets.Certifications[rng.Next(0, SampleDatasets.Certifications.Count-1)]);
                    numCertsGenerated++;
                }

                _client.Index(emp);
            }

            MessageBox.Show(string.Format("Created {0} employee records with {1} skills and {2} certs in index '{3}'", numEmployeesToCreate, numSkillsGenerated, numCertsGenerated, EsIndexName), "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cmdSearch_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            var resp = _client.Search<EmployeeSkillsDocument>(
                d => d.QueryString(txtSearch.Text)
                    .Highlight(h =>
                        h.OnFields(f =>
                            f.OnField(ef => ef.Skills))
                            .PreTags("<em>"))
                    .Aggregations(a =>
                        a.Terms("emp_skills_agg", t =>
                            t.Field(p => p.Skills)))
                );

            var resp2 = _client.Search<EmployeeSkillsDocument>(
                d => d.QueryString(txtSearch.Text)
                    .Aggregations(a =>
                        a.Terms("emp_skills_agg", t =>
                            t.Field(p => p.Skills)))
                );
            var rawQuery = Encoding.UTF8.GetString(resp2.RequestInformation.Request);
            txtQuery.Text = rawQuery;
            txtResults.Clear();
            txtResults.Text = ResultFormatter.FormatResults(resp2);
            txtCommonSkills.Text = ResultFormatter.FormatCommomSkills(resp2);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            txtSearch.SelectAll();
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Return)
            {
                PerformSearch();
            }
        }
    }
}

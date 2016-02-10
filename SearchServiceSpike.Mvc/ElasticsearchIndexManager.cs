using System;
using System.Collections.Generic;
using System.Linq;
using Nest;
using SQLite;
using WebApplication1.App_Data.@extern.models;
using WebApplication1.Controllers;
using WebApplication1.Models;

namespace WebApplication1
{
    public static class ElasticsearchIndexManager
    {
        private const string EsIndexName = "skills-spike";

        public static void GenerateLookupDocumentLists(ElasticClient client)
        {
            var i = 0;
            foreach (var skill in SampleDatasets.Skills)
            {
                client.Index(new Skill {Id = i++, Name = skill});
            }

            i = 0;
            foreach (var skill in SampleDatasets.Certifications)
            {
                client.Index(new Certification {Id = i++, Name = skill});
            }
        }

        public static string GenerateRandomEmployeeDocuments(ElasticClient client, string namesDbPath)
        {
            var db = new SQLiteConnection(namesDbPath);
            var female = db.Table<female>().ToList();
            var surname = db.Table<surname>().ToList();

            var rng = new Random();

            const int numEmployeesToCreate = 300;
            int numSkillsGenerated = 0;
            int numCertsGenerated = 0;
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
                    emp.Certifications.Add(
                        SampleDatasets.Certifications[rng.Next(0, SampleDatasets.Certifications.Count - 1)]);
                    numCertsGenerated++;
                }

                client.Index(emp);
            }

            return string.Format("Created {0} employee records with {1} skills and {2} certs in index '{3}'",
                numEmployeesToCreate, numSkillsGenerated, numCertsGenerated, EsIndexName);
        }

        public static void AddAutocompleteMappingToIndex(ElasticClient client)
        {
            //http://stackoverflow.com/questions/30285065/elasticsearch-nest-client-creating-multi-field-fields-with-completion
            client.CreateIndex(descriptor => descriptor
                .Index(EsIndexName)
                .AddMapping<Skill>(m => m
                    .Properties(p => p.MultiField(mf => mf
                        .Name(n => n.Name)
                        .Fields(f => f
                            .String(s => s.Name(n => n.Name).Index(FieldIndexOption.Analyzed))
                            .String(s => s.Name(n => n.Name.Suffix("sortable")).Index(FieldIndexOption.NotAnalyzed))
                            .String(s => s.Name(n => n.Name.Suffix("autocomplete")).IndexAnalyzer("shingle_analyzer"))))))
                .AddMapping<Certification>(m => m
                    .Properties(p => p.MultiField(mf => mf
                        .Name(n => n.Name)
                        .Fields(f => f
                            .String(s => s.Name(n => n.Name).Index(FieldIndexOption.Analyzed))
                            .String(s => s.Name(n => n.Name.Suffix("sortable")).Index(FieldIndexOption.NotAnalyzed))
                            .String(s => s.Name(n => n.Name.Suffix("autocomplete")).IndexAnalyzer("shingle_analyzer"))))))
                .Analysis(a => a
                    .Analyzers(b => b.Add("shingle_analyzer", new CustomAnalyzer
                    {
                        Tokenizer = "standard",
                        Filter = new List<string> {"lowercase", "shingle_filter"}
                    }))
                    .TokenFilters(b => b.Add("shingle_filter", new ShingleTokenFilter
                    {
                        MinShingleSize = 2,
                        MaxShingleSize = 5
                    })))
                );
        }

        private static void GenerateSuffixCompletionSearchMapping(ElasticClient client)
        {
            var createResult = client.CreateIndex(index => index
                .AddMapping<EmployeeSkillsDocument>(tmd => tmd
                    .Properties(props => props
                        .Completion(s =>
                            s.Name(p => p.Skills.Suffix("completion"))
                                .IndexAnalyzer("standard")
                                .SearchAnalyzer("standard")
                                .MaxInputLength(20)
                                .Payloads()
                                .PreservePositionIncrements()
                                .PreserveSeparators())
                    )
                )
                );
        }
    }
}
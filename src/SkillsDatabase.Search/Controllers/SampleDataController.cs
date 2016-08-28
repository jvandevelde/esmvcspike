using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.PlatformAbstractions;
using Nest;
using SkillsDatabase.Search.Api.Data;
using SkillsDatabase.Search.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SkillsDatabase.Search.Api.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController
    {
        private const string EsIndexName = "skills-spike";

        private IApplicationEnvironment _appEnv;
        private ElasticClient _esClient;
        private DataGenerator _dataGenerator;

        public SampleDataController(IApplicationEnvironment env, IElasticsearchClientProvider esClientProvider)
        {
            _appEnv = env;
            _esClient = esClientProvider.Instance;
            _dataGenerator = new DataGenerator();
        }

        [HttpPut("createlookups")]
        public void CreateLookups()
        {
            AddAutocompleteMappingsToIndex();

            foreach (var skill in _dataGenerator.SkillsCollection)
            {
                _esClient.Index(skill);
            }

            foreach (var cert in _dataGenerator.CertificationCollection)
            {
                _esClient.Index(cert);
            }
        }

        [HttpPut("createemployees")]
        public void CreateEmployees([FromUri]int number)
        {
            var data = _dataGenerator.TestCsvWithIntegers(_appEnv);
            var people = data.ToList();

            var rng = new Random();

            const int numEmployeesToCreate = 300;
            var numSkillsGenerated = 0;
            var numCertsGenerated = 0;
            for (var i = 0; i <= numEmployeesToCreate; i++)
            {
                var employee = people[rng.Next(0, people.Count() - 1)];

                var emp = new EmployeeSkillsDocument()
                {
                    Id = i,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Skills = new List<EmployeeSkill>(),
                    Certifications = new List<EmployeeCertification>()
                };

                for (var j = 0; j <= 10; j++)
                {
                    var candidateSkill = _dataGenerator.SkillsCollection[rng.Next(0, _dataGenerator.SkillsCollection.Count - 1)];
                    if (!emp.Skills.Select(s => s.Id).Contains(candidateSkill.Id))
                    {
                        emp.Skills.Add(new EmployeeSkill
                        {
                            Id = candidateSkill.Id,
                            Name = candidateSkill.Name,
                            YearsOfExperience = rng.Next(1, 10)
                        });
                        numSkillsGenerated++;
                    }
                }

                var numCerts = rng.Next(1, 10);
                var baseAchievementDate = new DateTime(1995, 1, 1);
                for (var k = 0; k <= numCerts; k++)
                {
                    var candidateSkill = _dataGenerator.CertificationCollection[rng.Next(0, _dataGenerator.CertificationCollection.Count - 1)];
                    if (!emp.Certifications.Select(c => c.Id).Contains(candidateSkill.Id))
                    {
                        emp.Certifications.Add(new EmployeeCertification
                        {
                            Id = candidateSkill.Id,
                            Name = candidateSkill.Name,
                            DateAchieved = baseAchievementDate.AddDays(rng.Next(1, 7300))
                        });
                        numCertsGenerated++;
                    }
                }

                _esClient.Index(emp);
            }
        }

        private void AddAutocompleteMappingsToIndex()
        {
            //http://stackoverflow.com/questions/30285065/elasticsearch-nest-client-creating-multi-field-fields-with-completion
            // Had to update the above to work with new 2.x NEST client and removal of multi-fields from ES 2.x
            // https://www.elastic.co/guide/en/elasticsearch/reference/current/_multi_fields.html
            _esClient.CreateIndex(EsIndexName, descriptor => 
                descriptor.Index(EsIndexName)
                    .Mappings(mappings => mappings.Map<Skill>(skillMap => skillMap
                        .Properties(props =>
                                props.String(p => p.Name(n => n.Name)
                                    .Fields(fields => fields
                                        .String(s => s.Name(n => n.Name).Index(FieldIndexOption.Analyzed))
                                        .String(s => s.Name(n => n.Name.Suffix("sortable")).Index(FieldIndexOption.NotAnalyzed))
                                        .String(s => s.Name(n => n.Name.Suffix("autocomplete")).Analyzer("shingle_analyzer"))))))
                    .Map<Certification>(certMap => certMap
                        .Properties(props => 
                                props.String(p => p.Name(n => n.Name)
                                    .Fields(fields => fields
                                        .String(s => s.Name(n => n.Name).Index(FieldIndexOption.Analyzed))
                                        .String(s => s.Name(n => n.Name.Suffix("sortable")).Index(FieldIndexOption.NotAnalyzed))
                                        .String(s => s.Name(n => n.Name.Suffix("autocomplete")).Analyzer("shingle_analyzer"))))))
                ).Settings(settings => settings.Analysis(a => a
                    .Analyzers(analyzers => analyzers
                        .Custom("shingle_analyzer", sa => sa
                            .Tokenizer("standard")
                            .Filters(new List<string> { "lowercase", "shingle_filter" })))
                    .TokenFilters(tFilters => tFilters
                        .Shingle("shingle_filter", stf => stf
                            .MinShingleSize(2)
                            .MaxShingleSize(5)))
                )));
        }

        private static void GenerateSuffixCompletionSearchMapping(ElasticClient client)
        {
            //var createResult = client.CreateIndex(index => index
            //    .AddMapping<EmployeeSkillsDocument>(tmd => tmd
            //        .Properties(props => props
            //            .Completion(s =>
            //                s.Name(p => p.Skills.Suffix("completion"))
            //                    .IndexAnalyzer("standard")
            //                    .SearchAnalyzer("standard")
            //                    .MaxInputLength(20)
            //                    .Payloads()
            //                    .PreservePositionIncrements()
            //                    .PreserveSeparators())
            //        )
            //    )
            //    );
        }
    }
}

using CsvToolkit;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using SkillsDatabase.Search.Api.Models;
using System;

namespace SkillsDatabase.Search.Api.Data
{
    //https://msdn.microsoft.com/library/mt695655.aspx  Update to VS Update 2 to fix debugger not showing variables
    public class DataGenerator
    {
        private static readonly IEnumerable<string> _skills = new List<string>
        {
            "Fort",
            "FoxPro",
            "C++",
            ".NET",
            "Analysis",
            "GitHub",
            "iOS",
            "Oracle",
            "Oracle - DBA",
            "Oracle - Financials",
            "Oracle - PL/SQL",
            "Oracle - Reports",
            "Oracle - SQL Development",
            "noSQL",
            "Stored Procedures",
            "Cognos",
            "Cognos BI Modeling",
            "Cognos Content Administration",
            "Cognos Impromptu",
            "Data Architecture",
            "Enterprise Application Integration",
            "Enterprise Architecture",
            "Enterprise Architecture Planning",
            "Enterprise Framework Design",
            "Microsoft Crystal Reports",
            "Microsoft Blend",
            "Microsoft .NET",
            ".NET",
            "Microsoft LINQ",
            "Microsoft Mediaroom Client",
            "Microsoft WPF",
            "Microsoft Windows Presentation Foundation",
            "XAML",
            "Microsoft XAML",
            "ADO",
            "ADO.NET",
            "LINQ",
            "LINQ to Sharepoint",
            "LINQ to CSV",
            "LINQ to SQL",
            "Entity Framework"
        };

        private static readonly IEnumerable<string> _certs = new List<string>
        {
            "App Dynamics Certified Expert",
            "AppDynamics Accredited Engnieer",
            "Associate of Arts in Computer Science",
            "Bachelor of Applied Information Systems Technology",
            "Bachelor of Applied Sciuence in Computer Engineering",
            "Bachelor of Applied Science in Industrial Engineering",
            "BMC",
            "BMC Sales Foundation",
            "BMC Remedy On Demand",
            "BMC ITSM Advanced Core Components",
            "CIP",
            "Certified Information Professional (CIP)",
            "Lotus - App Dev",
            "Certified Lotus Specialist",
            "Microsoft Biztalk Server 2010",
            "Microsoft Certified Application Developer",
            "Microsoft Certified Professional - Querying MS SQL Server 2012",
            "Microsoft Certified Professional (Development)",
            "Microsoft Certified Solutions Developer (MCSD)",
            "Microsoft Project Scheduler",
            "Sun Certified Web Component Developer",
            "Sun Certified Java Programmer",
            "Sonic MQ",
            "Sonic ESB"
        };

        public static List<string> Skills
        {
            get { return _skills.ToList(); }
        }

        public List<string> Certifications
        {
            get { return _certs.ToList(); }
        }

        public List<Skill> SkillsCollection
        {
            get
            {
                var rng = new Random();
                var i = 0;
                return _skills.Select(s =>
                {
                    var skill = new Skill { Id = i++, Name = s };
                    return skill;
                }).ToList();
            }
        }

        public List<Certification> CertificationCollection
        {
            get
            {
                var rng = new Random();
                var i = 0;
                return _certs.Select(c =>
                {
                    var skill = new Certification { Id = i++, Name = c };
                    return skill;
                }).ToList();
            }
        }

        public IEnumerable<dynamic> TestCsvWithIntegers(IApplicationEnvironment env)
        {
            var femaleRows = GetFileRows(env, "females.csv");
            var maleRows = GetFileRows(env, "males.csv");

            var females = new List<dynamic>();
            var males = new List<dynamic>();

            foreach(var row in femaleRows)
            {
                females.Add(new
                {
                    FirstName = row.GetColumn(0),
                    LastName = row.GetColumn(1),
                    Sex = row.GetColumn(2)
                });
            }
            
            foreach (var row in maleRows)
            {
                males.Add(new
                {
                    FirstName = row.GetColumn(0),
                    LastName = row.GetColumn(1),
                    Sex = row.GetColumn(2)
                });
            }

            return females.Concat(males);
        }

        private static string GetDataFilePath(IApplicationEnvironment env, string fileName)
        {
            var pathToFile = env.ApplicationBasePath
   + Path.DirectorySeparatorChar.ToString()
   + "Data"
   + Path.DirectorySeparatorChar.ToString()
   + fileName;

            return pathToFile;
        }

        private static CsvToolkit.Read.Row[] GetFileRows(IApplicationEnvironment env, string fileName)
        {
            var opts = new CsvReaderOptions()
            {
                FirstLineContainsHeaders = false,
                EndOfLineStyle = EndOfLineStyle.CrLf,
                SeparatorChar = ','
            };

            string fileContent;

            using (StreamReader freader = File.OpenText(GetDataFilePath(env, fileName)))
            {
                fileContent = freader.ReadToEnd();
            }

            var reader = CsvReader.Open(fileContent, opts);

            var rows = reader.GetRows().ToArray();

            return rows;
        }
    }
}

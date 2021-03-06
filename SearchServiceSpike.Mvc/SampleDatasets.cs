﻿using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Models;

namespace WebApplication1
{
    public static class SampleDatasets
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

        public static List<string> Certifications
        {
            get { return _certs.ToList(); }
        }

        public static List<Skill> SkillsCollection
        {
            get
            {
                var rng = new Random();
                var i = 0;
                return _skills.Select(s =>
                {
                    var skill = new Skill {Id = i++, Name = s, YearsOfExperience = rng.Next(1, 12)};
                    return skill;
                }).ToList();
            }
        }

        public static List<Certification> CertificationCollection
        {
            get
            {
                var rng = new Random();
                var i = 0;
                return _certs.Select(c =>
                {
                    var dateAchieved = new DateTime(rng.Next(2000, 2016), rng.Next(1, 12), rng.Next(1, 28));
                    var skill = new Certification {Id = i++, Name = c, DateAchieved = dateAchieved};
                    return skill;
                }).ToList();
            }
        }
    }
}

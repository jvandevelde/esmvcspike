using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class EmployeeSkillsDocument
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> Skills { get; set; }
        public List<string> Certifications { get; set; } 
    }
}
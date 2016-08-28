using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillsDatabase.Search.Api.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class EmployeeSkill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int YearsOfExperience { get; set; }

        protected bool Equals(EmployeeSkill other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EmployeeSkill)obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}

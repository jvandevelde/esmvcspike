using System;

namespace SkillsDatabase.Search.Api.Models
{
    public class EmployeeCompetency
    {
        private DateTime _dateAchieved;
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime DateAchieved
        {
            get { return _dateAchieved; }
            set { _dateAchieved = value.Date; }
        }

        protected bool Equals(EmployeeCertification other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EmployeeCertification)obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}

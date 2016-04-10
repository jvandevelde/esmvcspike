namespace WebApplication1.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int YearsOfExperience { get; set; }

        protected bool Equals(Skill other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Skill) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
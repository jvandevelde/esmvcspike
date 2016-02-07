namespace SkillsEsSpike.@extern.models
{
    /// <summary>
    /// Maps to the 'surname' table in the external names.db SQLLite database
    /// </summary>
    public class surname
    {
        public string name { get; set; }
        public double freq { get; set; }
    }
}
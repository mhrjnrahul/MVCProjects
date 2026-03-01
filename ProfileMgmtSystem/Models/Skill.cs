namespace ProfileMgmtSystem.Models
{
    public enum ProficiencyLevel
    {
        Beginner,
        Intermediate,
        Advanced,
        Expert
    }
    public class Skills : BaseEntity
    {
        //basic skill props
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public ProficiencyLevel Proficiency { get; set; }

        //navigation property for person
        public int PersonId { get; set; }
        public Person? Person { get; set; } = null;

        //overridden polymorphic describe method to provide a specific implementation for Skill
        public override string Describe()
        {
            return $"Skill: {Name} ({Proficiency})";
        }
    }
}

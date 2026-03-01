namespace ProfileMgmtSystem.Models
{
    public class Education : BaseEntity
    {
        public string? InstitutionName { get; set; }
        public string? Degree { get; set; }
        public string? FieldOfStudy { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }

        //foreign key to link education to a person
        public int PersonId { get; set; }
        public Person? Person { get; set; }

        //polymorphic override
        public override string Describe()
        {
            return $"Education: {Degree} in {FieldOfStudy} from {InstitutionName}";
        }
    }
}

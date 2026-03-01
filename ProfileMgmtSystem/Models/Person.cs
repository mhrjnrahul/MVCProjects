using System.Numerics;

namespace ProfileMgmtSystem.Models
{
    public class Person : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }

        //navigation properties for ef core
        //its like setting up relationships between tables in a databased 
        public ICollection<Education> Educations { get; set; } = new List<Education>();
        public ICollection<Skills> Skills { get; set; } = new List<Skills>();
        public ICollection<WorkExperience> Experiences { get; set; } = new List<WorkExperience>();

        //read-only property that are calculated
        public string? FullName => $"{FirstName} {LastName}";
        public int Age => DateTime.Now.Year - DateOfBirth.Year;

        //polymorphism: the ability of a method to do different things based on the object it is acting upon
        //overriding the Describe method from BaseEntity to provide a specific implementation for Person
        public override string Describe() =>

            $"Person: {FullName}, Email: {Email}, Age: {Age}";

    }
}

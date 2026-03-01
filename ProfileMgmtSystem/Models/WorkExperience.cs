using Microsoft.EntityFrameworkCore;
using ProfileMgmtSystem.Data;
using ProfileMgmtSystem.Models;

namespace ProfileMgmtSystem.Models
{
    public class WorkExperience : BaseEntity
    {
        //props
        public string? CompanyName { get; set; }
        public string? Position { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }

        //foreign key to link work experience to a person
        public int PersonId { get; set; }
        public Person? Person { get; set; }

        //overrided polymorphic describe method to provide a specific implementation for WorkExperience
        public override string Describe()
        {
            return $"WorkExperience: {CompanyName} - {Position} ({StartYear} - {EndYear})";
        }
    }
}

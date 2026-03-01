namespace ProfileMgmtSystem.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        //abstract method
        //cannot be instantiated directly, must be implemented by derived classes
        public abstract string Describe();
    }
}

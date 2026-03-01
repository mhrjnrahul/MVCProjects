using ProfileMgmtSystem.Models;

namespace ProfileMgmtSystem.Repositories
{
    public interface IUnitOfWork : IDisposable //we implement IDisposable to ensure that the resources
                                               //used by the unit of work are properly released when they are no longer needed
    {
        IRepository<Person> Persons { get; }
        IRepository<Education> Educations { get; }
        IRepository<Skills> Skills { get; }
        IRepository<WorkExperience> WorkExperiences { get; }
        Task<int> SaveAsync(); //we return a Task<int> because saving changes to the database is an asynchronous operation,
                               //and we want to be able to await it in our service layer
                               //the int return type indicates the number of state entries written to the database, which can be useful for error handling and logging purposes
    }
}

using ProfileMgmtSystem.Models;
using ProfileMgmtSystem.Data;

namespace ProfileMgmtSystem.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        //store the context first
        private readonly ProfileDbContext _context;

        //store the repositories for each entity type
        public IRepository<Person> Persons { get; }
        public IRepository<Education> Educations { get; }
        public IRepository<Skills> Skills { get; }
        public IRepository<WorkExperience> WorkExperiences { get; }

        //initialise the constructor to inject the context and create instances of the repositories
        //when we create an instance of UnitOfWork, we need to provide an instance of ProfileDbContext,
        //which will be used to create instances of the repositories for each entity type
        public UnitOfWork(ProfileDbContext context)
        {
            _context = context;
            Persons = new Repository<Person>(_context);
            Educations = new Repository<Education>(_context);
            Skills = new Repository<Skills>(_context);
            WorkExperiences = new Repository<WorkExperience>(_context);
        }

        //save all changes made through the repositories to the database in a single transaction
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        //dispose the context when the unit of work is disposed to release the resources used by the context
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using ProfileMgmtSystem.Data;
using ProfileMgmtSystem.Models;
using ProfileMgmtSystem.Repositories;

namespace ProfileMgmtSystem.Services
{
    public class PersonService
    {
        //create context field to access the database
        //inject the db context into the service through the constructor

        //private readonly ProfileDbContext _context; //do this when there is no repository pattern, and we want to access the db context directly in the service

        //whenever we create an instance of PersonService, we need to provide an instance of ProfileDbContext
        //this is done through dependency injection, which is a design pattern that allows us to manage dependencies between classes in a flexible and maintainable way
        //public PersonService(ProfileDbContext context)
        //{
        //    _context = context;
        //}

        //if we are using the repository pattern, we should inject the unit of work instead of the db context directly
        //store the unit of work to access the repositories for each entity type
        private readonly IUnitOfWork _unitOfWork;

        //inject the unit of work into the service through the constructor
        public PersonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //create service
        public async Task<Person> CreateAsync(string firstname, string lastname, string email, DateTime dob)
        { 
            var person = new Person
            {
                FirstName = firstname,
                LastName = lastname,
                Email = email,
                DateOfBirth = dob
            };
            //_context.Persons.Add(person); //we use this without repository pattern
            //await _context.SaveChangesAsync();

            //with repo pattern, we use the unit of work to access the repo for person entity and add new person to repo, the call the save method
            await _unitOfWork.Persons.AddAsync(person);
            await _unitOfWork.SaveAsync();
            return person;
        }


        //read service
        //read a single person by id
        //public async Task<Person?> GetByIdAsync(int id) => 

        //// await _context.Persons
        ////.Include(p => p.Educations)   // JOIN Educations //eager loading: load related data as part of the initial query to avoid multiple round-trips to the database
        ////.Include(p => p.Skills)        // JOIN Skills
        ////.Include(p => p.Experiences)
        ////.FirstOrDefaultAsync(p => p.Id == id);

        ////with repo pattern, we use the unit of work to access the repo for person entity and get
        ////the person by id, then we need to manually include the related data since the repository
        ////pattern abstracts away the db context and does not support eager loading by default
        //await _unitOfWork.Persons.GetByIdAsync(id); //get the person by id using the repo, this will not include the related data

        //read all person with include related data
        public async Task<Person?> GetByIdAsync(int id) =>
    await _unitOfWork.Persons.GetWithIncludesAsync(
        p => p.Id == id,
        p => p.Educations,
        p => p.Skills,
        p => p.Experiences
    );

        //list all persons
        public async Task<List<Person>> ListtAllAsync() => 
           // await _context.Persons.Include(p => p.Educations)   // JOIN Educations
           //.Include(p => p.Skills)        // JOIN Skills
           //.Include(p => p.Experiences)   // JOIN Experiences
           //.ToListAsync();

            //repo pattern
            await _unitOfWork.Persons.GetAllAsync(); //get all persons using the repo

        //update service
        //update a person's email by id
        public async Task<bool> UpdateAsync(int id, string? email = null, string? firstname = null)
        {
            //var person = await _context.Persons.FindAsync(id);
            var person = await _unitOfWork.Persons.GetByIdAsync(id); //get the person by id using the repo
            if (person == null)
            {
                return false;
            }

            if (email != null)
            {
                person.Email = email;
            }

            if (firstname != null)
            {
                person.FirstName = firstname;
            }

            person.UpdatedAt = DateTime.UtcNow;
            //await _context.SaveChangesAsync();

             _unitOfWork.Persons.Update(person); //update the person using the repo
            await _unitOfWork.SaveAsync(); 
            return true;
        }

        //delete service
        public async Task<bool> DeleteAsync(int id)
        {
            //var person = await _context.Persons.FindAsync(id);
            var person = await _unitOfWork.Persons.GetByIdAsync(id); //get the person by id using the repo
            if (person == null)
            {
                return false;
            }
            //_context.Persons.Remove(person); //this will also cascade delete the related educations and skills due to the configured relationships in the db context
            //await _context.SaveChangesAsync();

            _unitOfWork.Persons.Delete(person); //delete the person using the repo
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}

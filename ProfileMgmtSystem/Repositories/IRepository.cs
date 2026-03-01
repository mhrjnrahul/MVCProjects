using System.Linq.Expressions;

namespace ProfileMgmtSystem.Repositories
{
    //this is a generic interface
    //T is a type parameter that can be replaced with any type when the interface is implemented
    //this allows us to create a repository that can work with any type of entity, such as Person, Education, Skill, or WorkExperience
    //the IRepository interface defines the basic CRUD operations that can be performed on any entity type
    //by using a generic interface, we can avoid code duplication and promote code reusability, as we can implement the same interface for different entity types without having to write separate code for each type
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);//we return a Task<T?> because retrieving an entity from the database is an asynchronous operation,
                                      //and we want to be able to await it in our service layer.
                                      //The T? indicates that the method can return null if no entity with the specified id is found in the database

        Task<List<T>> GetAllAsync();//we return a Task<List<T>> because retrieving a list of entities from the database is an asynchronous operation,
                                    //and we want to be able to await it in our service layer

        Task<T?> GetWithIncludesAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes); 
        

        Task AddAsync(T entity);//we return a Task because adding an entity to the database is an asynchronous operation,
                                //and we want to be able to await it in our service layer
        void Update(T entity); //we dont return anything when we update an entity,
                               //we just perform the update operation on the database, and the
                               //changes will be saved when we call SaveChangesAsync() in the unit of work
        void Delete(T entity); //we dont return anything when we delete an entity, we just perform the delete operation on the database
    }
}

using Microsoft.EntityFrameworkCore;
using ProfileMgmtSystem.Data;
using System.Linq.Expressions;
namespace ProfileMgmtSystem.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        //store the context first
        private readonly ProfileDbContext _context;
        private readonly DbSet<T> _dbSet;

        //initialise the constructor to inject the context
        public Repository(ProfileDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>(); //get the DbSet for the specific entity type T
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetWithIncludesAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync(filter);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}

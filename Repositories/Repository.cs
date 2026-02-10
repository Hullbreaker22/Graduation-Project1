using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SkyLine.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _db;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _db = _context.Set<T>();
        }

        public async Task CreateAsync(T entity)
        {
            await _db.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _db.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            _db.Update(entity);
        }

        public void Delete(T entity)
        {
            _db.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _db.RemoveRange(entities);
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        
        public async Task<List<T>> GetAsync(
            Expression<Func<T, bool>>? expression = null,
            Expression<Func<T, object>>[]? includes = null,
            bool tracked = true)
        {
            var entities = _db.AsQueryable();

            if (expression is not null)
                entities = entities.Where(expression);

            if (includes is not null)
            {
                foreach (var item in includes)
                    entities = entities.Include(item);
            }

            if (!tracked)
                entities = entities.AsNoTracking();

            return await entities.ToListAsync();
        }

        public async Task<T?> GetOneAsync(
            Expression<Func<T, bool>> expression,
            Expression<Func<T, object>>[]? includes = null,
            bool tracked = true)
        {
            return (await GetAsync(expression, includes, tracked)).FirstOrDefault();
        }

        public async Task<List<T>> GetAllAsync(bool tracked = true)
        {
            var entities = tracked ? _db : _db.AsNoTracking();
            return await entities.ToListAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> expression)
        {
            return await _db.AnyAsync(expression);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? expression = null)
        {
            return expression is null
                ? await _db.CountAsync()
                : await _db.CountAsync(expression);
        }

        public async Task<T?> GetByIdAsync(params object[] keyValues)
        {
            return await _db.FindAsync(keyValues);
        }

        public void Detach(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }
    }
}

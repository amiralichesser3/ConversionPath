using ConversionPath.Domain.Contracts;
using ConversionPath.Persistence.Context; 
using Microsoft.EntityFrameworkCore;

namespace ConversionPath.Persistence
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : BaseEntity, IAggregateRoot
    {
        protected readonly AppDbContext _dbContext;
        protected readonly IDomainCollection<T> _domainCollection;

        public RepositoryBase(AppDbContext dbContext, IDomainCollection<T> domainCollection)
        {
            _dbContext = dbContext;
            _domainCollection = domainCollection;
        }

        public virtual async Task<ValidationResult<T>> Add(T entity)
        {
            var result = await _domainCollection.Add(entity);
            if (result.IsSuccessfull)
            {
                await _dbContext.Set<T>().AddAsync(entity); 
            }
            return result;
        } 

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            var memoryResult = _domainCollection.GetAll();
            if (memoryResult.Count == 0)
            {
                var dbRes = await _dbContext.Set<T>()
                    .OrderByDescending(r => r.Id)
                    .Take(_domainCollection.GetThreshold())
                    .ToListAsync();
                foreach (var item in dbRes)
                {
                    await _domainCollection.Add(item);
                }
            } 
            else if (memoryResult.Count <= _domainCollection.GetSize())
            {
                return memoryResult;
            }
            var dbResult = await _dbContext.Set<T>().ToListAsync(); 
            return dbResult.Union(memoryResult).Distinct<T>();
        }

        public virtual async Task<T> GetById(int id)
        {
            var memoryResult = _domainCollection.GetAll().FirstOrDefault(r => r.Id.Equals(id));
            if (memoryResult != null) return memoryResult;
            var dbResult = await _dbContext.Set<T>().FirstOrDefaultAsync(r => r.Id.Equals(id));
            return dbResult!;
        }

        public virtual async Task Remove(int id)
        {
            _domainCollection.Remove(id);
            var item = await GetById(id);
            _dbContext.Remove(item); 
        }
          
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync(); 
        }

        public abstract Task<ValidationResult<T>> Update(int id, T entity);
    }
}

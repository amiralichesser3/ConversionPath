
namespace ConversionPath.Domain.Contracts
{
    public interface IRepositoryBase<T> where T : BaseEntity, IAggregateRoot
    {
        public Task<T> GetById(int id);
        public Task<IEnumerable<T>> GetAll();
        public Task<ValidationResult<T>> Add(T entity);
        public Task Remove(int id);
        public Task<ValidationResult<T>> Update(int id, T entity);
        public Task SaveChangesAsync();
    }
}

namespace ConversionPath.Domain.Contracts
{
    public interface IDomainCollection<T> where T : BaseEntity
    { 
        IList<T> GetAll();
        int GetSize();
        Task<ValidationResult<T>> Add(T? customer);
        Task<ValidationResult<T>> Update(int id, T customer);
        bool Remove(int id);
        void Reset();
        bool ThresholdReached();
        
    }
}

using ConversionPath.Domain.Contracts; 
namespace ConversionPath.Domain.Contracts
{
    public abstract class DomainCollection<T> : IDomainCollection<T> where T : BaseEntity
    {
        public const int THRESHOLD = 1024;
        public IList<T> Items { get; }
        protected readonly IValidator<T> _validator;

        public DomainCollection(IValidator<T> validator)
        {
            Items = new List<T>();
            _validator = validator;
        }

        public async Task<ValidationResult<T>> Add(T? item)
        {
            var validationResult = await _validator.Validate(item);
            if (!validationResult.IsSuccessfull) return validationResult; 

            if (validationResult.IsSuccessfull)
            {
                Items.Add(item!);
            }

            // Preventing the collection to get too big! Only keeping top X in memory
            if (ThresholdReached())
            {
                Remove((Items.ToArray()[0].Id));
            }

            return validationResult;
        }

        public bool Remove(int id)
        {
            // Todo: Should check first if anything depends on this entity
            if (!Items.Any(r => r.Id == id)) return false;
            Items.Remove(Items.First(r => r.Id == id));
            return true;
        }

        public abstract Task<ValidationResult<T>> Update(int id, T item);

        public bool ThresholdReached()
        {
            return Items.Count > THRESHOLD;
        }

        public void Reset()
        {
            // Todo: Should check first if anything depends on this collection
            Items.Clear();
        }

        public IList<T> GetAll() => Items.ToList();

        public int GetSize() => Items.Count;
        public int GetThreshold() => THRESHOLD;
    }
}


namespace ConversionPath.Domain.Contracts
{
    public interface IValidator<T>
    {
        Task<ValidationResult<T>> Validate(T? input);
    }
}

namespace ConversionPath.Domain.Contracts
{
    public class ValidationResult<T>
    {
        public T? Data { get; set; }
        public ICollection<string> Messages { get; set; }
        public bool IsSuccessfull { get; set; }
    }
}
namespace ConversionPath.Shared.Dtos.ExchangeRates
{
    public class CreateOrUpdateExchangeRateDto
    {
        public double Rate { get; set; }
        public string SourceCurrency { get; set; }
        public string DestinationCurrency { get; set; }
        public DateTime DateTime { get; set; }
    }
}

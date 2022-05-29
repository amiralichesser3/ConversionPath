

using ConversionPath.Shared.Dtos.ExchangeRates;

namespace ConversionPath.Shared.Dtos;

public class ConversionResultDto
{
    public ConversionResultDto()
    {
        IsSucessfull = false;
        Result = 0;
        RatesUsed = new List<ExchangeRateDto>();
    }
    public bool IsSucessfull { get; set; }
    public double Result { get; set; }
    public ICollection<ExchangeRateDto> RatesUsed { get; set; }
}
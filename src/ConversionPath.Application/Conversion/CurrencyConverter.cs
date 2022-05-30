using ConversionPath.Application.ExchangeRates.Queries;
using ConversionPath.Domain.Contracts;
using ConversionPath.Domain.ExchangeRates.Entities;
using ConversionPath.Shared.Dtos;
using ConversionPath.Shared.Dtos.ExchangeRates;
using MediatR;

namespace ConversionPath.Application.Conversion;

public class CurrencyConverter: ICurrencyConverter
{
    private readonly IMediator _mediator;
    private ICollection<ExchangeRateDto> allRates;

    public CurrencyConverter(IMediator mediator)
    {
        allRates = new List<ExchangeRateDto>();
        _mediator = mediator;
    }

    public async Task<ConversionResultDto> Convert(string sourceCurrency, string destinationCurrency, double amount)
    {
        var result = new ConversionResultDto();
        if (!allRates.Any())
        {
            allRates = await _mediator.Send(new GetAllExchangeRatesQuery());
        } 
       
        var path = await FindPath(sourceCurrency, destinationCurrency);
        if (path.Any())
        {
            result.IsSucessfull = true;
            result.RatesUsed = allRates;
            if (path.Count == 1)
            {
                result.Result = path.First().Rate * amount;
            }
        }
        return result;
    }

    public void ResetRates()
    {
        allRates.Clear();
    }

    public void SetRates(ICollection<ExchangeRateDto> rates)
    {
        allRates = rates;
    }

    private async Task<ICollection<ExchangeRateDto>> FindPath(string sourceCurrency, string destinationCurrency, bool isReverse = false)
    {
        var sourceDestinationString = sourceCurrency.ToUpper() + "/" + destinationCurrency.ToUpper();
        var simpleRoute = allRates
            .FirstOrDefault(r => r.SourceDestinationString.Equals(sourceDestinationString));
        if (simpleRoute != null)
        { 
            return new List<ExchangeRateDto> { simpleRoute };
        }
        else
        {
            if (!isReverse)
            {
                return await FindPath(destinationCurrency, sourceCurrency, true);
            }

            return new List<ExchangeRateDto>(); 
        }
    }
}
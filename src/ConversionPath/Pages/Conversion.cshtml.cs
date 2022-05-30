using ConversionPath.Application.ExchangeRates.Queries;
using ConversionPath.Domain.Contracts;
using ConversionPath.Shared.Dtos;
using ConversionPath.Shared.Dtos.ExchangeRates;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConversionPath.Pages;

public class Conversion : PageModel
{
    private readonly ICurrencyConverter _converter;
    private readonly IMediator _mediator;

    private ICollection<ExchangeRateDto> _rates { get; set; }

    public Conversion(ICurrencyConverter converter, IMediator mediator)
    {
        _converter = converter;
        _mediator = mediator;
    }
    [BindProperty]
    public string SourceCurrency { get; set; }
    [BindProperty]
    public string DestinationCurrency { get; set; }
    [BindProperty]
    public double Amount { get; set; }
    
    public ConversionResultDto ConversionResult { get; set; }
    
    public async Task OnGetAsync()
    {
        SourceCurrency = "BNB";
        DestinationCurrency = "IRR";
        Amount = 0.5;

        _rates = await _mediator.Send(new GetAllExchangeRatesQuery());
        _converter.SetRates(_rates);
    }
    
    public async Task OnPostSubmit()
    {
        ConversionResult = await _converter.Convert(SourceCurrency, DestinationCurrency, Amount);
    }
}
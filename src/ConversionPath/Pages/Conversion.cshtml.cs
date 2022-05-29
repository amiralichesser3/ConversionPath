using ConversionPath.Domain.Contracts;
using ConversionPath.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConversionPath.Pages;

public class Conversion : PageModel
{
    private readonly ICurrencyConverter _converter;

    public Conversion(ICurrencyConverter converter)
    {
        _converter = converter;
    }
    [BindProperty]
    public string SourceCurrency { get; set; }
    [BindProperty]
    public string DestinationCurrency { get; set; }
    [BindProperty]
    public double Amount { get; set; }
    
    public ConversionResultDto ConversionResult { get; set; }
    
    public void OnGet()
    {
        SourceCurrency = "BNB";
        DestinationCurrency = "IRR";
        Amount = 0.5;
    }
    
    public async Task OnPostSubmit()
    {
        ConversionResult = await _converter.Convert(SourceCurrency, DestinationCurrency, Amount);
    }
}
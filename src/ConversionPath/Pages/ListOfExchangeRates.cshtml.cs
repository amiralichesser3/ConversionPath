using ConversionPath.Application.ExchangeRates.Queries;
using ConversionPath.Shared.Dtos.ExchangeRates;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConversionPath.Pages.ExchangeRates
{
    public class ListOfExchangeRatesModel : PageModel
    {
        protected readonly IMediator _mediator;
        public IEnumerable<ExchangeRateDto> Rates;

        public ListOfExchangeRatesModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task OnGetAsync()
        {
            Rates = await _mediator.Send(new GetAllExchangeRatesQuery());
        }
    }
}

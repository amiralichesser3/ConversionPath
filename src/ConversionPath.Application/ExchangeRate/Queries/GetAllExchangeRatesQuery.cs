using AutoMapper;
using ConversionPath.Domain.Contracts;
using ConversionPath.Domain.ExchangeRates.Entities;
using ConversionPath.Persistence.ExchangeRateAggregate.Repositories;
using ConversionPath.Shared.Dtos.ExchangeRates;
using MediatR;

namespace ConversionPath.Application.ExchangeRates.Queries
{
    public class GetAllExchangeRatesQuery : IRequest<ICollection<ExchangeRateDto>>
    { 
    }

    public class GetAllExchangeRatesQueryHandler : IRequestHandler<GetAllExchangeRatesQuery, ICollection<ExchangeRateDto>>
    {
        private readonly IRepositoryBase<ExchangeRate> _repo;
        private readonly IMapper _mapper;
        public GetAllExchangeRatesQueryHandler(IRepositoryBase<ExchangeRate> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ICollection<ExchangeRateDto>> Handle(GetAllExchangeRatesQuery request, CancellationToken cancellationToken)
        {
            var result = await _repo.GetAll(); 
            if (!result.Any())
            {
                var seedData = new List<ExchangeRate>();
                seedData.Add(new ExchangeRate { SourceCurrency = "USD", DestinationCurrency = "IRR", DateTime = DateTime.Now, Rate = 42400 });
                seedData.Add(new ExchangeRate { SourceCurrency = "USD", DestinationCurrency = "BTC", DateTime = DateTime.Now, Rate = 0.000034 });
                seedData.Add(new ExchangeRate { SourceCurrency = "EUR", DestinationCurrency = "USD", DateTime = DateTime.Now, Rate = 1.07 });
                seedData.Add(new ExchangeRate { SourceCurrency = "ETH", DestinationCurrency = "EUR", DateTime = DateTime.Now, Rate = 1671725 });
                seedData.Add(new ExchangeRate { SourceCurrency = "GBP", DestinationCurrency = "USD", DateTime = DateTime.Now, Rate = 1.26 });
                seedData.Add(new ExchangeRate { SourceCurrency = "ETH", DestinationCurrency = "BNB", DateTime = DateTime.Now, Rate = 0.1688 });
                seedData.Add(new ExchangeRate { SourceCurrency = "TRX", DestinationCurrency = "BNB", DateTime = DateTime.Now, Rate = 0.1688 });
                seedData.Add(new ExchangeRate { SourceCurrency = "USDT", DestinationCurrency = "TRX", DateTime = DateTime.Now, Rate = 0.1688 });

                foreach (var item in seedData)
                {
                    await _repo.Add(item);
                }
                result = await _repo.GetAll();
            }

            return _mapper.Map<ICollection<ExchangeRateDto>>(result);
        }
    }
}

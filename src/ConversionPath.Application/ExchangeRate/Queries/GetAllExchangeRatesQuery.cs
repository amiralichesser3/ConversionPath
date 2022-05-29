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
            return _mapper.Map<ICollection<ExchangeRateDto>>(result);
        }
    }
}

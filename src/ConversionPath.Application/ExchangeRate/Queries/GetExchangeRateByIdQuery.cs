using AutoMapper;
using ConversionPath.Domain.Contracts;
using ConversionPath.Domain.ExchangeRates.Entities;
using ConversionPath.Persistence.ExchangeRateAggregate.Repositories;
using ConversionPath.Shared.Dtos.ExchangeRates;
using MediatR;

namespace ConversionPath.Application.ExchangeRates.Queries
{
    public class GetExchangeRateByIdQuery : IRequest<ExchangeRateDto>
    {
        public int Id { get; set; }
    }

    public class GetExchangeRateByIdQueryHandler : IRequestHandler<GetExchangeRateByIdQuery, ExchangeRateDto>
    {
        private readonly IRepositoryBase<ExchangeRate> _repo;
        private readonly IMapper _mapper;
        public GetExchangeRateByIdQueryHandler(IRepositoryBase<ExchangeRate> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ExchangeRateDto> Handle(GetExchangeRateByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _repo.GetById(request.Id);
            return _mapper.Map<ExchangeRateDto>(result);
        }
    }
}

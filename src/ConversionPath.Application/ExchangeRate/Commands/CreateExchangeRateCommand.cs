using AutoMapper;
using ConversionPath.Domain.Contracts;
using ConversionPath.Domain.ExchangeRates.Entities;
using ConversionPath.Shared.Dtos;
using ConversionPath.Shared.Dtos.ExchangeRates;
using MediatR;

namespace ConversionPath.Application.ExchangeRates.Commands
{
    public class CreateExchangeRateCommand : IRequest<ValidationResultDto<ExchangeRateDto>>
    {
        public CreateOrUpdateExchangeRateDto ExchangeRate { get; set; }
    }

    public class CreateExchangeRateCommandHandler : IRequestHandler<CreateExchangeRateCommand, ValidationResultDto<ExchangeRateDto>>
    {
        private readonly IRepositoryBase<ExchangeRate> _repo;
        private readonly IMapper _mapper;
        public CreateExchangeRateCommandHandler(IRepositoryBase<ExchangeRate> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ValidationResultDto<ExchangeRateDto>> Handle(CreateExchangeRateCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<ExchangeRate>(request.ExchangeRate);
            var result = await _repo.Add(entity);
            if (result.IsSuccessfull)
            {
                await _repo.SaveChangesAsync();
            }
            var resultDto = _mapper.Map<ValidationResultDto<ExchangeRateDto>>(result);
            return resultDto;
        }
    }
}

using AutoMapper;
using ConversionPath.Domain.Contracts;
using ConversionPath.Domain.ExchangeRates.Entities;
using ConversionPath.Shared.Dtos;
using ConversionPath.Shared.Dtos.ExchangeRates;
using MediatR;

namespace ConversionPath.Application.ExchangeRates.Commands
{
    public class UpdateExchangeRateCommand : IRequest<ValidationResultDto<ExchangeRateDto>>
    {
        public int Id { get; set; }
        public CreateOrUpdateExchangeRateDto ExchangeRate { get; set; }
    }

    public class UpdateExchangeRateCommandHandler : IRequestHandler<UpdateExchangeRateCommand, ValidationResultDto<ExchangeRateDto>>
    { 
        private readonly IRepositoryBase<ExchangeRate> _repo;
        private readonly IMapper _mapper;
        public UpdateExchangeRateCommandHandler(IRepositoryBase<ExchangeRate> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ValidationResultDto<ExchangeRateDto>> Handle(UpdateExchangeRateCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<ExchangeRate>(request.ExchangeRate); 
            var result = await _repo.Update(request.Id, entity);
            if (result.IsSuccessfull)
            {
                await _repo.SaveChangesAsync();
            }
            var resultDto = _mapper.Map<ValidationResultDto<ExchangeRateDto>>(result);
            return resultDto;
        }
    }
}

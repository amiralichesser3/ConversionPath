using AutoMapper;
using ConversionPath.Domain.Contracts;
using ConversionPath.Domain.ExchangeRates.Entities;
using MediatR;

namespace ConversionPath.Application.ExchangeRates.Commands
{
    public class DeleteExchangeRateCommand : IRequest<bool>
    {
        public int Id { get; set; } 
    }

    public class DeleteExchangeRateCommandHandler : IRequestHandler<DeleteExchangeRateCommand, bool>
    {
        private readonly IRepositoryBase<ExchangeRate> _repo;
        private readonly IMapper _mapper;
        public DeleteExchangeRateCommandHandler(IRepositoryBase<ExchangeRate> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<bool> Handle(DeleteExchangeRateCommand request, CancellationToken cancellationToken)
        { 
            await _repo.Remove(request.Id);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}

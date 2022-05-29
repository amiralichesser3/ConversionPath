using AutoMapper;
using ConversionPath.Domain.ExchangeRates.Entities;
using ConversionPath.Shared.Dtos.ExchangeRates;

namespace ConversionPath.MappingProfile;
public class ExchangeRateDtoProfile : Profile
{
    public ExchangeRateDtoProfile()
    {
        CreateMap<ExchangeRateDto, ExchangeRate>().ReverseMap()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
    public override string ProfileName => "CustomerDtoProfile";
}

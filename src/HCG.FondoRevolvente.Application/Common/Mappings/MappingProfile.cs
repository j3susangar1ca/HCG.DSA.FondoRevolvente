using AutoMapper;
using HCG.FondoRevolvente.Domain.Aggregates;
using HCG.FondoRevolvente.Application.Solicitudes.Queries.GetSolicitudes;

namespace HCG.FondoRevolvente.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Solicitud, SolicitudDto>()
            .ForMember(d => d.Folio, opt => opt.MapFrom(s => s.Folio.Valor))
            .ForMember(d => d.Monto, opt => opt.MapFrom(s => s.Monto.Valor))
            .ForMember(d => d.Estado, opt => opt.MapFrom(s => s.Estado.ToString()));
    }
}

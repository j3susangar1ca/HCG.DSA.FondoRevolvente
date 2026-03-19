using MediatR;
using HCG.FondoRevolvente.Application.Common.Models;
using HCG.FondoRevolvente.Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace HCG.FondoRevolvente.Application.Solicitudes.Queries.GetSolicitudes;

public class GetSolicitudesQueryHandler : IRequestHandler<GetSolicitudesQuery, PaginatedList<SolicitudDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSolicitudesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<SolicitudDto>> Handle(GetSolicitudesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Solicitudes
            .AsNoTracking()
            .OrderByDescending(x => x.FechaCreacion)
            .ProjectTo<SolicitudDto>(_mapper.ConfigurationProvider);

        var count = await query.CountAsync(cancellationToken);
        
        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<SolicitudDto>(items, count, request.PageNumber, request.PageSize);
    }
}

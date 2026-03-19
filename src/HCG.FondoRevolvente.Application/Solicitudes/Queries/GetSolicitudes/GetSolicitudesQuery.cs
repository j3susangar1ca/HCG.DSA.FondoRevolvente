using MediatR;
using HCG.FondoRevolvente.Application.Common.Models;

namespace HCG.FondoRevolvente.Application.Solicitudes.Queries.GetSolicitudes;

public record GetSolicitudesQuery : IRequest<PaginatedList<SolicitudDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

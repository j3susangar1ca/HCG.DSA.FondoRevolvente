using MediatR;
using HCG.FondoRevolvente.Application.Common.Models;

namespace HCG.FondoRevolvente.Application.Solicitudes.Commands.CrearSolicitud;

public record CrearSolicitudCommand : IRequest<Result<int>>
{
    public decimal Monto { get; init; }
    public string Descripcion { get; init; } = null!;
}

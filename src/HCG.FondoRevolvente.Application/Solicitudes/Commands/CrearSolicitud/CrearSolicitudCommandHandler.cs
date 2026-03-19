using MediatR;

namespace HCG.FondoRevolvente.Application.Solicitudes.Commands.CrearSolicitud;

public class CrearSolicitudCommand : IRequest<CrearSolicitudResponse> {}
public class CrearSolicitudResponse { public int Id { get; set; } }

public class CrearSolicitudCommandHandler : IRequestHandler<CrearSolicitudCommand, CrearSolicitudResponse>
{
    public async Task<CrearSolicitudResponse> Handle(CrearSolicitudCommand request, CancellationToken cancellationToken)
    {
        return new CrearSolicitudResponse { Id = 1 };
    }
}

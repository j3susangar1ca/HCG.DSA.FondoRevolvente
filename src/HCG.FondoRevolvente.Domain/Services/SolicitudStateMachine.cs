using HCG.FondoRevolvente.Domain.Aggregates;
using HCG.FondoRevolvente.Domain.Enums;
using HCG.FondoRevolvente.Domain.Interfaces;
using HCG.FondoRevolvente.Domain.Exceptions;

namespace HCG.FondoRevolvente.Domain.Services;

/// <summary>
/// Implementación de la lógica de transición de estados.
/// §Módulo 06 README.
/// </summary>
public class SolicitudStateMachine : ISolicitudStateMachine
{
    public bool CanTransitionTo(EstadoSolicitud current, EstadoSolicitud next)
    {
        return (current, next) switch
        {
            (EstadoSolicitud.Borrador, EstadoSolicitud.PendienteValidacion) => true,
            (EstadoSolicitud.PendienteValidacion, EstadoSolicitud.Validada) => true,
            (EstadoSolicitud.PendienteValidacion, EstadoSolicitud.Rechazada) => true,
            (EstadoSolicitud.Validada, EstadoSolicitud.EnTramitePago) => true,
            (EstadoSolicitud.EnTramitePago, EstadoSolicitud.Pagada) => true,
            (_, EstadoSolicitud.ErrorSatReintentando) => true,
            (EstadoSolicitud.ErrorSatReintentando, EstadoSolicitud.Validada) => true,
            _ => false
        };
    }

    public void TransitionTo(Solicitud solicitud, EstadoSolicitud nuevoEstado, string usuario, string? motivo = null)
    {
        if (!CanTransitionTo(solicitud.Estado, nuevoEstado))
            throw new DomainException($"Transición de estado no permitida: {solicitud.Estado} -> {nuevoEstado}");

        solicitud.CambiarEstado(nuevoEstado, usuario, motivo);
    }
}

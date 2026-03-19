using HCG.FondoRevolvente.Domain.Aggregates;
using HCG.FondoRevolvente.Domain.Enums;

namespace HCG.FondoRevolvente.Domain.Interfaces;

/// <summary>
/// Contrato para el motor de estados de las solicitudes.
/// Centraliza las reglas de transición permitidas.
/// </summary>
public interface ISolicitudStateMachine
{
    /// <summary>
    /// Determina si es posible transitar al nuevo estado desde el actual.
    /// </summary>
    bool CanTransitionTo(EstadoSolicitud currentState, EstadoSolicitud newState);

    /// <summary>
    /// Ejecuta la transición de estado en la solicitud, validando precondiciones.
    /// </summary>
    void TransitionTo(Solicitud solicitud, EstadoSolicitud nuevoEstado, string usuario, string? motivo = null);
}

using HCG.FondoRevolvente.Domain.Aggregates;
using HCG.FondoRevolvente.Domain.Enums;
using HCG.FondoRevolvente.Domain.Interfaces;
using HCG.FondoRevolvente.Domain.Exceptions;

namespace HCG.FondoRevolvente.Domain.Services;

/// <summary>
/// Implementación de la lógica de transición de estados.
/// Governa las 38 transiciones permitidas entre los 30 estados.
/// §Módulo 06 y Sección 1.2 README.
/// </summary>
public class SolicitudStateMachine : ISolicitudStateMachine
{
    public bool CanTransitionTo(EstadoSolicitud current, EstadoSolicitud next)
    {
        // Regla general: Se puede cancelar casi desde cualquier estado no final
        if (next == EstadoSolicitud.Cancelado && !current.EsEstadoFinal())
            return true;

        // Regla general: Error de SAT es posible desde validación CFDI
        if (next == EstadoSolicitud.ErrorSatReintentando && current == EstadoSolicitud.EnValidacionCfdi)
            return true;

        return (current, next) switch
        {
            #region Fase 1 -> 2 o 3
            (EstadoSolicitud.Recepcionado, EstadoSolicitud.EnRevision) => true,
            (EstadoSolicitud.EnRevision, EstadoSolicitud.Validado) => true,
            (EstadoSolicitud.EnRevision, EstadoSolicitud.RechazadoValidacion) => true,
            (EstadoSolicitud.EnRevision, EstadoSolicitud.EnFraccionamiento) => true,
            (EstadoSolicitud.EnFraccionamiento, EstadoSolicitud.Validado) => true,
            (EstadoSolicitud.EnFraccionamiento, EstadoSolicitud.RechazadoValidacion) => true,
            
            // Si validado, puede ir a CAA o directo a Cotización según monto (Lógica en Application Service)
            (EstadoSolicitud.Validado, EstadoSolicitud.EnAutorizacionCAA) => true,
            (EstadoSolicitud.Validado, EstadoSolicitud.SinCotizaciones) => true,
            #endregion

            #region Fase 2 (CAA)
            (EstadoSolicitud.EnAutorizacionCAA, EstadoSolicitud.AutorizadoCAA) => true,
            (EstadoSolicitud.EnAutorizacionCAA, EstadoSolicitud.RechazadoCAA) => true,
            (EstadoSolicitud.EnAutorizacionCAA, EstadoSolicitud.RechazadoCAAReintento) => true,
            (EstadoSolicitud.AutorizadoCAA, EstadoSolicitud.SinCotizaciones) => true,
            (EstadoSolicitud.RechazadoCAAReintento, EstadoSolicitud.EnRevision) => true, // Permitir corregir y volver a validar
            #endregion

            #region Fase 3 (Cotización)
            (EstadoSolicitud.SinCotizaciones, EstadoSolicitud.EnCotizacion) => true,
            (EstadoSolicitud.EnCotizacion, EstadoSolicitud.CotizacionCompleta) => true,
            (EstadoSolicitud.EnCotizacion, EstadoSolicitud.SinCotizaciones) => true, // Si se borran cotizaciones
            #endregion

            #region Fase 4 (Selección/Pedido)
            (EstadoSolicitud.CotizacionCompleta, EstadoSolicitud.CuadroComparativo) => true,
            (EstadoSolicitud.CuadroComparativo, EstadoSolicitud.ProveedorSeleccionado) => true,
            (EstadoSolicitud.ProveedorSeleccionado, EstadoSolicitud.PedidoGenerado) => true,
            #endregion

            #region Fase 5 (Entrega)
            (EstadoSolicitud.PedidoGenerado, EstadoSolicitud.EnEntrega) => true,
            (EstadoSolicitud.EnEntrega, EstadoSolicitud.Entregado) => true,
            (EstadoSolicitud.EnEntrega, EstadoSolicitud.ConDiscrepancia) => true,
            (EstadoSolicitud.Entregado, EstadoSolicitud.EnRecepcionBienes) => true,
            (EstadoSolicitud.ConDiscrepancia, EstadoSolicitud.EnEntrega) => true, // Reintento tras corregir discrepancia
            (EstadoSolicitud.EnRecepcionBienes, EstadoSolicitud.RecepcionadoBienes) => true,
            #endregion

            #region Fase 6 (CFDI)
            (EstadoSolicitud.RecepcionadoBienes, EstadoSolicitud.EnValidacionCfdi) => true,
            (EstadoSolicitud.EnValidacionCfdi, EstadoSolicitud.CfdiValido) => true,
            (EstadoSolicitud.EnValidacionCfdi, EstadoSolicitud.CfdiInvalido) => true,
            (EstadoSolicitud.ErrorSatReintentando, EstadoSolicitud.CfdiValido) => true,
            (EstadoSolicitud.ErrorSatReintentando, EstadoSolicitud.CfdiInvalido) => true,
            (EstadoSolicitud.CfdiInvalido, EstadoSolicitud.EnValidacionCfdi) => true, // Reintento con nuevo XML
            #endregion

            #region Fase 7 (Pago)
            (EstadoSolicitud.CfdiValido, EstadoSolicitud.EnPago) => true,
            (EstadoSolicitud.EnPago, EstadoSolicitud.Pagado) => true,
            (EstadoSolicitud.EnPago, EstadoSolicitud.ErrorPago) => true,
            (EstadoSolicitud.ErrorPago, EstadoSolicitud.EnPago) => true, // Reintento de pago
            #endregion

            #region Fase 8 (Cierre)
            (EstadoSolicitud.Pagado, EstadoSolicitud.EnCierre) => true,
            (EstadoSolicitud.EnCierre, EstadoSolicitud.Cerrado) => true,
            #endregion

            _ => false
        };
    }

    public void TransitionTo(Solicitud solicitud, EstadoSolicitud nuevoEstado, string usuario, string? motivo = null)
    {
        if (!CanTransitionTo(solicitud.Estado, nuevoEstado))
            throw new DomainException($"Transición de estado denegada operacionalmente: {solicitud.Estado} -> {nuevoEstado}. " +
                                     $"Consulte la Sección 1.2 de la matriz de transiciones.");

        solicitud.CambiarEstado(nuevoEstado, usuario, motivo);
    }
}

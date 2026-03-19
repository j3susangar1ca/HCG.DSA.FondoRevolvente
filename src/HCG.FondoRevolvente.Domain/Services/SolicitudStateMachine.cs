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
    private static readonly List<TransicionEstado> _transiciones = InitTransiciones();

    public bool PuedeTransicionar(EstadoSolicitud estadoActual, EstadoSolicitud estadoDestino)
    {
        // Regla general: Se puede cancelar casi desde cualquier estado no final
        if (estadoDestino == EstadoSolicitud.Cancelado && !estadoActual.EsEstadoFinal())
            return true;

        // Regla general: Error de SAT es posible desde validación CFDI
        if (estadoDestino == EstadoSolicitud.ErrorSatReintentando && estadoActual == EstadoSolicitud.EnValidacionCfdi)
            return true;

        return _transiciones.Any(t => t.EstadoOrigen == estadoActual && t.EstadoDestino == estadoDestino);
    }

    public IEnumerable<EstadoSolicitud> ObtenerEstadosPosibles(EstadoSolicitud estadoActual)
    {
        var posibles = _transiciones
            .Where(t => t.EstadoOrigen == estadoActual)
            .Select(t => t.EstadoDestino)
            .ToList();

        // Siempre se puede cancelar si no es estado final
        if (!estadoActual.EsEstadoFinal() && !posibles.Contains(EstadoSolicitud.Cancelado))
            posibles.Add(EstadoSolicitud.Cancelado);

        return posibles;
    }

    public IEnumerable<TransicionEstado> ObtenerTransicionesValidas(EstadoSolicitud estadoActual)
    {
        return _transiciones.Where(t => t.EstadoOrigen == estadoActual);
    }

    public bool ValidarTransicion(EstadoSolicitud estadoActual, EstadoSolicitud estadoDestino, out string? error)
    {
        error = null;

        if (PuedeTransicionar(estadoActual, estadoDestino))
            return true;

        error = $"Transición de estado denegada: {estadoActual} -> {estadoDestino}. " +
                "Consulte la Sección 1.2 de la matriz de transiciones.";
        return false;
    }

    public TransicionEstado? ObtenerTransicion(EstadoSolicitud origen, EstadoSolicitud destino)
    {
        return _transiciones.FirstOrDefault(t => t.EstadoOrigen == origen && t.EstadoDestino == destino);
    }

    public IReadOnlyList<TransicionEstado> ObtenerTodasLasTransiciones()
    {
        return _transiciones.AsReadOnly();
    }

    private static List<TransicionEstado> InitTransiciones()
    {
        var admin = new[] { RolAplicacion.Administrador };
        var comprador = new[] { RolAplicacion.CompradorDSA };
        var caa = new[] { RolAplicacion.RevisorCAA };
        var finanzas = new[] { RolAplicacion.Finanzas };
        var almacen = new[] { RolAplicacion.Almacen };
        var compradorAdmin = new[] { RolAplicacion.CompradorDSA, RolAplicacion.Administrador };

        return new List<TransicionEstado>
        {
            // Fase 1: Recepción y Validación
            new(EstadoSolicitud.Recepcionado, EstadoSolicitud.EnRevision, "Iniciar revisión", "Se inicia la revisión del oficio", comprador),
            new(EstadoSolicitud.EnRevision, EstadoSolicitud.Validado, "Validar", "Solicitud validada correctamente", comprador),
            new(EstadoSolicitud.EnRevision, EstadoSolicitud.RechazadoValidacion, "Rechazar", "Solicitud rechazada en validación", comprador, requiereComentario: true),
            new(EstadoSolicitud.EnRevision, EstadoSolicitud.EnFraccionamiento, "Detectar fraccionamiento", "Se detectó posible fraccionamiento", comprador),
            new(EstadoSolicitud.EnFraccionamiento, EstadoSolicitud.Validado, "Aprobar pese fraccionamiento", "Aprobada tras análisis de fraccionamiento", compradorAdmin),
            new(EstadoSolicitud.EnFraccionamiento, EstadoSolicitud.RechazadoValidacion, "Rechazar por fraccionamiento", "Rechazada por fraccionamiento", compradorAdmin, requiereComentario: true),
            new(EstadoSolicitud.Validado, EstadoSolicitud.EnAutorizacionCAA, "Enviar a CAA", "Enviada al Comité CAA", comprador),
            new(EstadoSolicitud.Validado, EstadoSolicitud.SinCotizaciones, "Enviar a cotización", "Enviada directamente a cotización", comprador),

            // Fase 2: Autorización CAA
            new(EstadoSolicitud.EnAutorizacionCAA, EstadoSolicitud.AutorizadoCAA, "Autorizar CAA", "Autorizada por el CAA", caa),
            new(EstadoSolicitud.EnAutorizacionCAA, EstadoSolicitud.RechazadoCAA, "Rechazar CAA", "Rechazada por el CAA", caa, requiereComentario: true),
            new(EstadoSolicitud.EnAutorizacionCAA, EstadoSolicitud.RechazadoCAAReintento, "Rechazar con reintento", "Rechazada con posibilidad de corrección", caa, requiereComentario: true),
            new(EstadoSolicitud.AutorizadoCAA, EstadoSolicitud.SinCotizaciones, "Enviar a cotización", "Enviada a cotización tras autorización CAA", comprador),
            new(EstadoSolicitud.RechazadoCAAReintento, EstadoSolicitud.EnRevision, "Corregir y reenviar", "Corregida para reenvío", comprador),

            // Fase 3: Cotización
            new(EstadoSolicitud.SinCotizaciones, EstadoSolicitud.EnCotizacion, "Registrar cotización", "Se inició el registro de cotizaciones", comprador),
            new(EstadoSolicitud.EnCotizacion, EstadoSolicitud.CotizacionCompleta, "Completar cotizaciones", "Se completaron las cotizaciones mínimas", comprador),
            new(EstadoSolicitud.EnCotizacion, EstadoSolicitud.SinCotizaciones, "Eliminar cotizaciones", "Se eliminaron las cotizaciones", comprador),

            // Fase 4: Selección / Pedido
            new(EstadoSolicitud.CotizacionCompleta, EstadoSolicitud.CuadroComparativo, "Generar cuadro comparativo", "Se generó el cuadro comparativo", comprador),
            new(EstadoSolicitud.CuadroComparativo, EstadoSolicitud.ProveedorSeleccionado, "Seleccionar proveedor", "Se seleccionó el proveedor ganador", comprador),
            new(EstadoSolicitud.ProveedorSeleccionado, EstadoSolicitud.PedidoGenerado, "Generar pedido", "Se generó el pedido", comprador),

            // Fase 5: Entrega
            new(EstadoSolicitud.PedidoGenerado, EstadoSolicitud.EnEntrega, "Enviar a entrega", "Se envió a entrega", comprador),
            new(EstadoSolicitud.EnEntrega, EstadoSolicitud.Entregado, "Confirmar entrega", "Entrega confirmada", almacen),
            new(EstadoSolicitud.EnEntrega, EstadoSolicitud.ConDiscrepancia, "Reportar discrepancia", "Se encontró discrepancia en la entrega", almacen, requiereComentario: true),
            new(EstadoSolicitud.Entregado, EstadoSolicitud.EnRecepcionBienes, "Recibir bienes", "Bienes en recepción", almacen),
            new(EstadoSolicitud.ConDiscrepancia, EstadoSolicitud.EnEntrega, "Resolver discrepancia", "Discrepancia resuelta", almacen),
            new(EstadoSolicitud.EnRecepcionBienes, EstadoSolicitud.RecepcionadoBienes, "Confirmar recepción", "Bienes recepcionados", almacen),

            // Fase 6: Validación Fiscal CFDI
            new(EstadoSolicitud.RecepcionadoBienes, EstadoSolicitud.EnValidacionCfdi, "Iniciar validación CFDI", "Se inició la validación fiscal", finanzas),
            new(EstadoSolicitud.EnValidacionCfdi, EstadoSolicitud.CfdiValido, "Validar CFDI", "CFDI validado correctamente", finanzas, esAutomatica: true),
            new(EstadoSolicitud.EnValidacionCfdi, EstadoSolicitud.CfdiInvalido, "Rechazar CFDI", "CFDI rechazado por SAT", finanzas, esAutomatica: true),
            new(EstadoSolicitud.ErrorSatReintentando, EstadoSolicitud.CfdiValido, "Validar CFDI tras reintento", "CFDI válido tras reintento", finanzas, esAutomatica: true),
            new(EstadoSolicitud.ErrorSatReintentando, EstadoSolicitud.CfdiInvalido, "Rechazar CFDI tras reintento", "CFDI rechazado tras reintento", finanzas, esAutomatica: true),
            new(EstadoSolicitud.CfdiInvalido, EstadoSolicitud.EnValidacionCfdi, "Reintentar con nuevo CFDI", "Nuevo CFDI cargado para validación", finanzas),

            // Fase 7: Pago
            new(EstadoSolicitud.CfdiValido, EstadoSolicitud.EnPago, "Iniciar pago", "Se inició el proceso de pago", finanzas),
            new(EstadoSolicitud.EnPago, EstadoSolicitud.Pagado, "Confirmar pago", "Pago realizado exitosamente", finanzas),
            new(EstadoSolicitud.EnPago, EstadoSolicitud.ErrorPago, "Error pago", "Error al procesar el pago", finanzas),
            new(EstadoSolicitud.ErrorPago, EstadoSolicitud.EnPago, "Reintentar pago", "Reintento de pago", finanzas),

            // Fase 8: Cierre
            new(EstadoSolicitud.Pagado, EstadoSolicitud.EnCierre, "Iniciar cierre", "Se inició el cierre del expediente", comprador),
            new(EstadoSolicitud.EnCierre, EstadoSolicitud.Cerrado, "Cerrar expediente", "Expediente cerrado oficialmente", compradorAdmin),
        };
    }
}

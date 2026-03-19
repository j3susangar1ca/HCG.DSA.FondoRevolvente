using HCG.FondoRevolvente.Domain.Enums;
using HCG.FondoRevolvente.Domain.Interfaces;

namespace HCG.FondoRevolvente.Domain.Services;

/// <summary>
/// Servicio de dominio que implementa la máquina de estados de solicitudes.
/// Define y gestiona las 38 transiciones válidas entre los 30 estados.
/// Este servicio es stateless y puede ser registrado como Singleton.
/// </summary>
public class SolicitudStateMachine : ISolicitudStateMachine
{
    /// <summary>
    /// Diccionario que mapea cada estado origen a sus posibles transiciones.
    /// </summary>
    private readonly Dictionary<EstadoSolicitud, List<TransicionEstado>> _transicionesPorOrigen;

    /// <summary>
    /// Lista completa de todas las transiciones definidas.
    /// </summary>
    private readonly IReadOnlyList<TransicionEstado> _todasLasTransiciones;

    public SolicitudStateMachine()
    {
        _todasLasTransiciones = DefinirTransiciones();
        _transicionesPorOrigen = _todasLasTransiciones
            .GroupBy(t => t.EstadoOrigen)
            .ToDictionary(g => g.Key, g => g.ToList());
    }

    #region Definición de las 38 Transiciones

    /// <summary>
    /// Define todas las transiciones válidas del sistema.
    /// Total: 38 transiciones entre 30 estados.
    /// </summary>
    private static List<TransicionEstado> DefinirTransiciones()
    {
        var transiciones = new List<TransicionEstado>
        {
            #region Fase 1: Recepción y Validación Inicial (5 transiciones)

            // Recepcionado -> EnRevision (iniciar revisión)
            new(
                EstadoSolicitud.Recepcionado,
                EstadoSolicitud.EnRevision,
                "IniciarRevision",
                "Iniciar la revisión documental de la solicitud",
                [RolAplicacion.Administrador, RolAplicacion.CompradorDSA]
            ),

            // Recepcionado -> Cancelado
            new(
                EstadoSolicitud.Recepcionado,
                EstadoSolicitud.Cancelado,
                "Cancelar",
                "Cancelar la solicitud",
                [RolAplicacion.Administrador, RolAplicacion.CompradorDSA],
                requiereComentario: true
            ),

            // EnRevision -> Validado
            new(
                EstadoSolicitud.EnRevision,
                EstadoSolicitud.Validado,
                "Validar",
                "La solicitud ha sido validada correctamente",
                [RolAplicacion.Administrador, RolAplicacion.CompradorDSA]
            ),

            // EnRevision -> RechazadoValidacion
            new(
                EstadoSolicitud.EnRevision,
                EstadoSolicitud.RechazadoValidacion,
                "Rechazar",
                "Rechazar la solicitud en validación",
                [RolAplicacion.Administrador, RolAplicacion.CompradorDSA],
                requiereComentario: true
            ),

            // EnRevision -> EnFraccionamiento
            new(
                EstadoSolicitud.EnRevision,
                EstadoSolicitud.EnFraccionamiento,
                "DetectarFraccionamiento",
                "Se detectó posible fraccionamiento de compra",
                [RolAplicacion.Administrador, RolAplicacion.CompradorDSA]
            ),

            // EnFraccionamiento -> Validado
            new(
                EstadoSolicitud.EnFraccionamiento,
                EstadoSolicitud.Validado,
                "AprobarPeseFraccionamiento",
                "Aprobada tras análisis de fraccionamiento",
                [RolAplicacion.Administrador, RolAplicacion.CompradorDSA]
            ),

            // EnFraccionamiento -> RechazadoValidacion
            new(
                EstadoSolicitud.EnFraccionamiento,
                EstadoSolicitud.RechazadoValidacion,
                "RechazarPorFraccionamiento",
                "Rechazada por fraccionamiento confirmado",
                [RolAplicacion.Administrador, RolAplicacion.CompradorDSA],
                requiereComentario: true
            ),

            // Validado -> EnAutorizacionCAA (si requiere CAA)
            new(
                EstadoSolicitud.Validado,
                EstadoSolicitud.EnAutorizacionCAA,
                "EnviarACaa",
                "Enviar al Comité CAA para autorización",
                [RolAplicacion.Administrador, RolAplicacion.CompradorDSA]
            ),

            // Validado -> SinCotizaciones (si no requiere CAA)
            new(
                EstadoSolicitud.Validado,
                EstadoSolicitud.SinCotizaciones,
                "EnviarACotizacion",
                "Enviar directamente a proceso de cotización",
                [RolAplicacion.Administrador, RolAplicacion.CompradorDSA]
            ),

            #endregion

            #region Fase 2: Autorización CAA (6 transiciones)

            // EnAutorizacionCAA -> AutorizadoCAA
            new(
                EstadoSolicitud.EnAutorizacionCAA,
                EstadoSolicitud.AutorizadoCAA,
                "AutorizarCaa",
                "Autorizar la solicitud en el CAA",
                [RolAplicacion.Administrador, RolAplicacion.RevisorCAA]
            ),

            // EnAutorizacionCAA -> RechazadoCAA
            new(
                EstadoSolicitud.EnAutorizacionCAA,
                EstadoSolicitud.RechazadoCAA,
                "RechazarCaa",
                "Rechazar la solicitud en el CAA",
                [RolAplicacion.Administrador, RolAplicacion.RevisorCAA],
                requiereComentario: true
            ),

            // EnAutorizacionCAA -> RechazadoCAAReintento
            new(
                EstadoSolicitud.EnAutorizacionCAA,
                EstadoSolicitud.RechazadoCAAReintento,
                "RechazarConReintento",
                "Rechazar con posibilidad de corrección",
                [RolAplicacion.Administrador, RolAplicacion.RevisorCAA],
                requiereComentario: true
            ),

            // AutorizadoCAA -> SinCotizaciones
            new(
                EstadoSolicitud.AutorizadoCAA,
                EstadoSolicitud.SinCotizaciones,
                "EnviarACotizacion",
                "Enviar a proceso de cotización tras autorización CAA",
                [RolAplicacion.Administrador, RolAplicacion.CompradorDSA]
            ),

            // RechazadoCAAReintento -> EnRevision (corrección y reenvío)
            new(
                EstadoSolicitud.RechazadoCAAReintento,
                EstadoSolicitud.EnRevision,
                "CorregirYReenviar",
                "Corregir y reenviar la solicitud",
                [RolAplicacion.Administrador, RolAplicacion.CompradorDSA]
            ),

            #endregion

            #region Fase 3: Estudio de Mercado / Cotización (4 transiciones)

            // SinCotizaciones -> EnCotizacion
            new(
                EstadoSolicitud.SinCotizaciones,
                EstadoSolicitud.EnCotizacion,
                "RegistrarCotizacion",
                "Se inició el registro de cotizaciones",
                [RolAplicacion.Administrador, RolAplicacion.CompradorDSA]
            ),

            // EnCotizacion -> CotizacionCompleta
            new(
                EstadoSolicitud.EnCotizacion,
                EstadoSolicitud.CotizacionCompleta,
                "CompletarCotizaciones",
                "Se completaron las cotizaciones mínimas requeridas",
                [RolAplicacion.Administrador, RolAplicacion.CompradorDSA],
                condiciones: "Mínimo 3 cotizaciones vigentes"
            ),

            // EnCotizacion -> SinCotizaciones (si se borran)
            new(
                EstadoSolicitud.EnCotizacion,
                EstadoSolicitud.SinCotizaciones,
                "EliminarCotizaciones",
                "Se eliminaron las cotizaciones",
                [RolAplicacion.Administrador, RolAplicacion.CompradorDSA]
            ),

            // EnCotizacion -> Cancelado
            new(
                EstadoSolicitud.EnCotizacion,
                EstadoSolicitud.Cancelado,
                "Cancelar",
                "Cancelar la solicitud durante cotización",
                [RolAplicacion.Administrador, RolAplicacion.CompradorDSA],
                requiereComentario: true
            ),

            #endregion

            #region Fase 4: Selección de Proveedor / Pedido (3 transiciones)

            // CotizacionCompleta -> CuadroComparativo
            new(
                EstadoSolicitud.CotizacionCompleta,
                EstadoSolicitud.CuadroComparativo,
                "GenerarCuadro",
                "Generar cuadro comparativo de cotizaciones",
                [RolAplicacion.Administrador, RolAplicacion.CompradorDSA]
            ),

            // CuadroComparativo -> ProveedorSeleccionado
            new(
                EstadoSolicitud.CuadroComparativo,
                EstadoSolicitud.ProveedorSeleccionado,
                "SeleccionarProveedor",
                "Seleccionar al proveedor ganador",
                [RolAplicacion.Administrador, RolAplicacion.CompradorDSA],
                condiciones: "Debe seleccionar una cotización vigente"
            ),

            // ProveedorSeleccionado -> PedidoGenerado
            new(
                EstadoSolicitud.ProveedorSeleccionado,
                EstadoSolicitud.PedidoGenerado,
                "GenerarPedido",
                "Generar pedido al proveedor",
                [RolAplicacion.Administrador, RolAplicacion.CompradorDSA]
            ),

            #endregion

            #region Fase 5: Entrega de Bienes (6 transiciones)

            // PedidoGenerado -> EnEntrega
            new(
                EstadoSolicitud.PedidoGenerado,
                EstadoSolicitud.EnEntrega,
                "EnviarAEntrega",
                "Proveedor confirmó y está en proceso de entrega",
                [RolAplicacion.Administrador, RolAplicacion.CompradorDSA]
            ),

            // EnEntrega -> Entregado
            new(
                EstadoSolicitud.EnEntrega,
                EstadoSolicitud.Entregado,
                "ConfirmarEntrega",
                "Bienes/servicios entregados por el proveedor",
                [RolAplicacion.Administrador, RolAplicacion.Almacen]
            ),

            // EnEntrega -> ConDiscrepancia
            new(
                EstadoSolicitud.EnEntrega,
                EstadoSolicitud.ConDiscrepancia,
                "ReportarDiscrepancia",
                "Se detectaron discrepancias en la entrega",
                [RolAplicacion.Administrador, RolAplicacion.Almacen],
                requiereComentario: true
            ),

            // Entregado -> EnRecepcionBienes
            new(
                EstadoSolicitud.Entregado,
                EstadoSolicitud.EnRecepcionBienes,
                "RecibirBienes",
                "Bienes en proceso de recepción en almacén",
                [RolAplicacion.Administrador, RolAplicacion.Almacen]
            ),

            // ConDiscrepancia -> EnEntrega (resolver)
            new(
                EstadoSolicitud.ConDiscrepancia,
                EstadoSolicitud.EnEntrega,
                "ResolverDiscrepancia",
                "Discrepancia resuelta, continuando entrega",
                [RolAplicacion.Administrador, RolAplicacion.Almacen]
            ),

            // EnRecepcionBienes -> RecepcionadoBienes
            new(
                EstadoSolicitud.EnRecepcionBienes,
                EstadoSolicitud.RecepcionadoBienes,
                "ConfirmarRecepcion",
                "Bienes recepcionados satisfactoriamente",
                [RolAplicacion.Administrador, RolAplicacion.Almacen]
            ),

            #endregion

            #region Fase 6: Validación Fiscal CFDI (6 transiciones)

            // RecepcionadoBienes -> EnValidacionCfdi
            new(
                EstadoSolicitud.RecepcionadoBienes,
                EstadoSolicitud.EnValidacionCfdi,
                "IniciarValidacionCfdi",
                "Iniciar validación fiscal del CFDI",
                [RolAplicacion.Administrador, RolAplicacion.Finanzas]
            ),

            // EnValidacionCfdi -> CfdiValido
            new(
                EstadoSolicitud.EnValidacionCfdi,
                EstadoSolicitud.CfdiValido,
                "ValidarCfdi",
                "CFDI validado exitosamente ante el SAT",
                [RolAplicacion.Administrador, RolAplicacion.Finanzas],
                esAutomatica: true
            ),

            // EnValidacionCfdi -> CfdiInvalido
            new(
                EstadoSolicitud.EnValidacionCfdi,
                EstadoSolicitud.CfdiInvalido,
                "RechazarCfdi",
                "CFDI rechazado por el SAT",
                [RolAplicacion.Administrador, RolAplicacion.Finanzas],
                esAutomatica: true
            ),

            // EnValidacionCfdi -> ErrorSatReintentando
            new(
                EstadoSolicitud.EnValidacionCfdi,
                EstadoSolicitud.ErrorSatReintentando,
                "ErrorSat",
                "Error de conexión SAT, programando reintento",
                [RolAplicacion.Administrador, RolAplicacion.Finanzas],
                esAutomatica: true
            ),

            // ErrorSatReintentando -> CfdiValido
            new(
                EstadoSolicitud.ErrorSatReintentando,
                EstadoSolicitud.CfdiValido,
                "ValidarTrasReintento",
                "CFDI válido tras reintento",
                [RolAplicacion.Administrador, RolAplicacion.Finanzas],
                esAutomatica: true
            ),

            // ErrorSatReintentando -> CfdiInvalido
            new(
                EstadoSolicitud.ErrorSatReintentando,
                EstadoSolicitud.CfdiInvalido,
                "RechazarTrasReintento",
                "CFDI rechazado tras reintento",
                [RolAplicacion.Administrador, RolAplicacion.Finanzas],
                esAutomatica: true
            ),

            // CfdiInvalido -> EnValidacionCfdi (con nuevo CFDI)
            new(
                EstadoSolicitud.CfdiInvalido,
                EstadoSolicitud.EnValidacionCfdi,
                "ReintentarConNuevoCfdi",
                "Reintentar validación con nuevo CFDI",
                [RolAplicacion.Administrador, RolAplicacion.Finanzas]
            ),

            #endregion

            #region Fase 7: Pago (4 transiciones)

            // CfdiValido -> EnPago
            new(
                EstadoSolicitud.CfdiValido,
                EstadoSolicitud.EnPago,
                "IniciarPago",
                "Iniciar trámite de pago",
                [RolAplicacion.Administrador, RolAplicacion.Finanzas]
            ),

            // EnPago -> Pagado
            new(
                EstadoSolicitud.EnPago,
                EstadoSolicitud.Pagado,
                "ConfirmarPago",
                "Pago realizado exitosamente",
                [RolAplicacion.Administrador, RolAplicacion.Finanzas]
            ),

            // EnPago -> ErrorPago
            new(
                EstadoSolicitud.EnPago,
                EstadoSolicitud.ErrorPago,
                "ErrorPago",
                "Error durante el proceso de pago",
                [RolAplicacion.Administrador, RolAplicacion.Finanzas]
            ),

            // ErrorPago -> EnPago (reintento)
            new(
                EstadoSolicitud.ErrorPago,
                EstadoSolicitud.EnPago,
                "ReintentarPago",
                "Reintentar proceso de pago",
                [RolAplicacion.Administrador, RolAplicacion.Finanzas]
            ),

            #endregion

            #region Fase 8: Cierre (2 transiciones)

            // Pagado -> EnCierre
            new(
                EstadoSolicitud.Pagado,
                EstadoSolicitud.EnCierre,
                "IniciarCierre",
                "Iniciar el cierre del expediente",
                [RolAplicacion.Administrador, RolAplicacion.CompradorDSA]
            ),

            // EnCierre -> Cerrado
            new(
                EstadoSolicitud.EnCierre,
                EstadoSolicitud.Cerrado,
                "CerrarExpediente",
                "Cerrar el expediente oficialmente",
                [RolAplicacion.Administrador, RolAplicacion.CompradorDSA]
            )

            #endregion
        };

        return transiciones;
    }

    #endregion

    #region Implementación de ISolicitudStateMachine

    /// <inheritdoc />
    public bool PuedeTransicionar(EstadoSolicitud estadoActual, EstadoSolicitud estadoDestino)
    {
        // Regla general: se puede cancelar desde cualquier estado no final
        if (estadoDestino == EstadoSolicitud.Cancelado && !estadoActual.EsEstadoFinal())
            return true;

        if (_transicionesPorOrigen.TryGetValue(estadoActual, out var transiciones))
        {
            return transiciones.Any(t => t.EstadoDestino == estadoDestino);
        }
        return false;
    }

    /// <inheritdoc />
    public IEnumerable<EstadoSolicitud> ObtenerEstadosPosibles(EstadoSolicitud estadoActual)
    {
        var posibles = new List<EstadoSolicitud>();

        if (_transicionesPorOrigen.TryGetValue(estadoActual, out var transiciones))
        {
            posibles.AddRange(transiciones.Select(t => t.EstadoDestino).Distinct());
        }

        // Siempre se puede cancelar si no es estado final
        if (!estadoActual.EsEstadoFinal() && !posibles.Contains(EstadoSolicitud.Cancelado))
            posibles.Add(EstadoSolicitud.Cancelado);

        return posibles;
    }

    /// <inheritdoc />
    public IEnumerable<TransicionEstado> ObtenerTransicionesValidas(EstadoSolicitud estadoActual)
    {
        if (_transicionesPorOrigen.TryGetValue(estadoActual, out var transiciones))
        {
            return transiciones;
        }
        return Enumerable.Empty<TransicionEstado>();
    }

    /// <inheritdoc />
    public bool ValidarTransicion(EstadoSolicitud estadoActual, EstadoSolicitud estadoDestino, out string? error)
    {
        error = null;

        // Estados terminales no permiten transiciones
        if (estadoActual.EsEstadoFinal())
        {
            error = $"El estado '{estadoActual}' es terminal y no permite más transiciones.";
            return false;
        }

        // Buscar la transición específica
        if (PuedeTransicionar(estadoActual, estadoDestino))
            return true;

        var estadosPosibles = string.Join(", ", ObtenerEstadosPosibles(estadoActual));
        error = $"No existe transición válida de '{estadoActual}' a '{estadoDestino}'. " +
                $"Estados posibles: {estadosPosibles}";
        return false;
    }

    /// <inheritdoc />
    public TransicionEstado? ObtenerTransicion(EstadoSolicitud origen, EstadoSolicitud destino)
    {
        if (_transicionesPorOrigen.TryGetValue(origen, out var transiciones))
        {
            return transiciones.FirstOrDefault(t => t.EstadoDestino == destino);
        }
        return null;
    }

    /// <inheritdoc />
    public IReadOnlyList<TransicionEstado> ObtenerTodasLasTransiciones() => _todasLasTransiciones;

    #endregion

    #region Métodos de Consulta Adicionales

    /// <summary>
    /// Obtiene el número total de transiciones definidas.
    /// </summary>
    public int TotalTransiciones => _todasLasTransiciones.Count;

    /// <summary>
    /// Obtiene todas las transiciones que un rol específico puede ejecutar.
    /// </summary>
    public IEnumerable<TransicionEstado> ObtenerTransicionesPorRol(RolAplicacion rol)
    {
        return _todasLasTransiciones
            .Where(t => t.RolesPermitidos.Contains(rol) || rol == RolAplicacion.Administrador);
    }

    /// <summary>
    /// Obtiene todas las transiciones de una fase específica.
    /// </summary>
    public IEnumerable<TransicionEstado> ObtenerTransicionesPorFase(FaseProceso fase)
    {
        return _todasLasTransiciones
            .Where(t => t.EstadoOrigen.ObtenerFase() == fase);
    }

    /// <summary>
    /// Obtiene un resumen del flujo de estados para visualización.
    /// </summary>
    public string ObtenerResumenFlujo()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("=== Flujo de Estados del Fondo Revolvente ===");
        sb.AppendLine($"Total de estados: 30");
        sb.AppendLine($"Total de transiciones: {TotalTransiciones}");
        sb.AppendLine();

        foreach (var fase in Enum.GetValues<FaseProceso>())
        {
            var transicionesFase = ObtenerTransicionesPorFase(fase).ToList();
            if (transicionesFase.Count > 0)
            {
                sb.AppendLine($"[Fase {(int)fase}] - {transicionesFase.Count} transiciones");
                foreach (var t in transicionesFase)
                {
                    sb.AppendLine($"  {t.EstadoOrigen} -> {t.EstadoDestino} ({t.Accion})");
                }
                sb.AppendLine();
            }
        }

        return sb.ToString();
    }

    #endregion
}

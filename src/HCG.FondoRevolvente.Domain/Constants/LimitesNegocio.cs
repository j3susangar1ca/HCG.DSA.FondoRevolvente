namespace HCG.FondoRevolvente.Domain.Constants;

/// <summary>
/// Fuente única de verdad para los límites normativos del sistema.
/// Referencia: Ley de Compras del Estado de Jalisco, Art. 57 del Reglamento.
/// </summary>
public static class LimitesNegocio
{
    /// <summary>
    /// RN-001 — Monto máximo por operación de Fondo Revolvente.
    /// $75,000.00 MXN. No puede modificarse en código; requiere cambio legislativo.
    /// Configurable en §Módulo 13 (Panel de Administración) solo por Administrador,
    /// con doble confirmación y registro en auditoría.
    /// </summary>
    public const decimal MontoMaximoFondoRevolvente = 75_000.00m;

    /// <summary>
    /// RN-003 — Número mínimo de cotizaciones de proveedores distintos
    /// requeridas antes de poder seleccionar proveedor ganador.
    /// </summary>
    public const int CotizacionesMinimas = 3;

    /// <summary>
    /// RN-005 — Tiempo de vida del bloqueo de edición en minutos.
    /// Se renueva automáticamente cada 25 min mientras la sesión está activa.
    /// </summary>
    public const int BloqueoEdicionTtlMinutos = 30;

    /// <summary>Umbral para alerta amarilla en la barra de progreso del MontoDisplay (70%).</summary>
    public const double UmbralAlertaAmarilla = 0.70;

    /// <summary>Umbral para alerta roja en la barra de progreso del MontoDisplay (90%).</summary>
    public const double UmbralAlertaRoja = 0.90;
}

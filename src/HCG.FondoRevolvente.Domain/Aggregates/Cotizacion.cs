using HCG.FondoRevolvente.Domain.Exceptions;
using HCG.FondoRevolvente.Domain.ValueObjects;

namespace HCG.FondoRevolvente.Domain.Aggregates;

/// <summary>
/// Representa una cotización enviada por un proveedor para una solicitud de adquisición.
/// Cada solicitud debe tener mínimo 3 cotizaciones según RN-003.
/// </summary>
public class Cotizacion
{
    #region Propiedades de Identidad

    /// <summary>
    /// Identificador único de la cotización.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// ID de la solicitud a la que pertenece esta cotización.
    /// </summary>
    public int SolicitudId { get; private set; }

    /// <summary>
    /// Referencia a la solicitud (navegación).
    /// </summary>
    public Solicitud? Solicitud { get; private set; }

    /// <summary>
    /// ID del proveedor que envió la cotización.
    /// </summary>
    public int ProveedorId { get; private set; }

    /// <summary>
    /// Referencia al proveedor (navegación).
    /// </summary>
    public Proveedor? Proveedor { get; private set; }

    #endregion

    #region Propiedades de la Cotización

    /// <summary>
    /// Número o folio de la cotización asignado por el proveedor.
    /// </summary>
    public string NumeroCotizacion { get; private set; } = null!;

    /// <summary>
    /// Monto cotizado por el proveedor (sin IVA).
    /// </summary>
    public MontoFondoRevolvente MontoSubtotal { get; private set; } = null!;

    /// <summary>
    /// Monto del IVA aplicado.
    /// </summary>
    public decimal MontoIva { get; private set; }

    /// <summary>
    /// Monto total de la cotización (subtotal + IVA).
    /// </summary>
    public MontoFondoRevolvente MontoTotal { get; private set; } = null!;

    /// <summary>
    /// Tasa de IVA aplicada (0.16 estándar, 0.08 zona fronteriza, 0 exento).
    /// </summary>
    public decimal TasaIva { get; private set; }

    /// <summary>
    /// Fecha de emisión de la cotización por parte del proveedor.
    /// </summary>
    public DateTime FechaEmision { get; private set; }

    /// <summary>
    /// Fecha de vigencia de la cotización (fecha hasta la cual es válida).
    /// </summary>
    public DateTime FechaVigencia { get; private set; }

    /// <summary>
    /// Fecha en que se recibió la cotización en el sistema.
    /// </summary>
    public DateTime FechaRecepcion { get; private set; }

    /// <summary>
    /// Condiciones de entrega ofrecidas por el proveedor.
    /// </summary>
    public string? CondicionesEntrega { get; private set; }

    /// <summary>
    /// Tiempo de entrega prometido en días naturales.
    /// </summary>
    public int? DiasEntrega { get; private set; }

    /// <summary>
    /// Observaciones o notas incluidas en la cotización.
    /// </summary>
    public string? Observaciones { get; private set; }

    /// <summary>
    /// Ruta al archivo PDF de la cotización en el repositorio SMB.
    /// </summary>
    public string? RutaArchivoPdf { get; private set; }

    #endregion

    #region Propiedades de Estado

    /// <summary>
    /// Indica si esta cotización fue seleccionada como la ganadora.
    /// </summary>
    public bool Seleccionada { get; private set; }

    /// <summary>
    /// Indica si la cotización está vigente (no ha expirado).
    /// </summary>
    public bool EstaVigente => DateTime.UtcNow <= FechaVigencia;

    /// <summary>
    /// Fecha en que se seleccionó esta cotización como ganadora.
    /// </summary>
    public DateTime? FechaSeleccion { get; private set; }

    /// <summary>
    /// Usuario que registró la cotización en el sistema.
    /// </summary>
    public string? UsuarioRegistro { get; private set; }

    /// <summary>
    /// Razón por la cual fue seleccionada o rechazada esta cotización.
    /// </summary>
    public string? RazonSeleccion { get; private set; }

    #endregion

    #region Constructores

    // Constructor para EF Core
    private Cotizacion() { }

    /// <summary>
    /// Crea una nueva instancia de cotización.
    /// </summary>
    public Cotizacion(
        int solicitudId,
        int proveedorId,
        string numeroCotizacion,
        MontoFondoRevolvente montoSubtotal,
        decimal tasaIva,
        DateTime fechaEmision,
        DateTime fechaVigencia,
        string? usuarioRegistro = null)
    {
        if (string.IsNullOrWhiteSpace(numeroCotizacion))
            throw new DomainException("El número de cotización es requerido.");

        if (fechaVigencia < fechaEmision)
            throw new DomainException("La fecha de vigencia no puede ser anterior a la fecha de emisión.");

        SolicitudId = solicitudId;
        ProveedorId = proveedorId;
        NumeroCotizacion = numeroCotizacion.Trim();
        MontoSubtotal = montoSubtotal ?? throw new DomainException("El monto subtotal es requerido.");
        TasaIva = tasaIva;
        MontoIva = Math.Round(montoSubtotal.Valor * tasaIva, 2, MidpointRounding.AwayFromZero);

        // El monto total debe pasar las mismas validaciones del límite
        var montoTotalValor = montoSubtotal.Valor + MontoIva;
        MontoTotal = MontoFondoRevolvente.Crear(montoTotalValor);

        FechaEmision = fechaEmision;
        FechaVigencia = fechaVigencia;
        FechaRecepcion = DateTime.UtcNow;
        UsuarioRegistro = usuarioRegistro;
        Seleccionada = false;
    }

    #endregion

    #region Métodos de Dominio

    /// <summary>
    /// Actualiza los datos de la cotización.
    /// Solo permitido si la cotización no ha sido seleccionada.
    /// </summary>
    public void Actualizar(
        string numeroCotizacion,
        MontoFondoRevolvente montoSubtotal,
        decimal tasaIva,
        DateTime fechaEmision,
        DateTime fechaVigencia,
        string? condicionesEntrega,
        int? diasEntrega,
        string? observaciones)
    {
        if (Seleccionada)
            throw new DomainException("No se puede modificar una cotización que ya fue seleccionada.");

        if (string.IsNullOrWhiteSpace(numeroCotizacion))
            throw new DomainException("El número de cotización es requerido.");

        if (fechaVigencia < fechaEmision)
            throw new DomainException("La fecha de vigencia no puede ser anterior a la fecha de emisión.");

        NumeroCotizacion = numeroCotizacion.Trim();
        MontoSubtotal = montoSubtotal ?? throw new DomainException("El monto subtotal es requerido.");
        TasaIva = tasaIva;
        MontoIva = Math.Round(montoSubtotal.Valor * tasaIva, 2, MidpointRounding.AwayFromZero);

        var montoTotalValor = montoSubtotal.Valor + MontoIva;
        MontoTotal = MontoFondoRevolvente.Crear(montoTotalValor);

        FechaEmision = fechaEmision;
        FechaVigencia = fechaVigencia;
        CondicionesEntrega = condicionesEntrega?.Trim();
        DiasEntrega = diasEntrega;
        Observaciones = observaciones?.Trim();
    }

    /// <summary>
    /// Establece la ruta del archivo PDF de la cotización.
    /// </summary>
    public void EstablecerRutaArchivoPdf(string rutaArchivo)
    {
        if (string.IsNullOrWhiteSpace(rutaArchivo))
            throw new DomainException("La ruta del archivo no puede estar vacía.");

        RutaArchivoPdf = rutaArchivo;
    }

    /// <summary>
    /// Actualiza las condiciones de entrega y tiempo prometido.
    /// </summary>
    public void EstablecerCondicionesEntrega(string? condiciones, int? diasEntrega)
    {
        CondicionesEntrega = condiciones?.Trim();
        DiasEntrega = diasEntrega;
    }

    /// <summary>
    /// Selecciona esta cotización como la ganadora.
    /// </summary>
    internal void SeleccionarComoGanadora(string? razon = null)
    {
        if (!EstaVigente)
            throw new DomainException("No se puede seleccionar una cotización que ha expirado.");

        Seleccionada = true;
        FechaSeleccion = DateTime.UtcNow;
        RazonSeleccion = razon;
    }

    /// <summary>
    /// Deselecciona esta cotización (si había sido seleccionada por error).
    /// </summary>
    internal void Deseleccionar()
    {
        Seleccionada = false;
        FechaSeleccion = null;
        RazonSeleccion = null;
    }

    /// <summary>
    /// Calcula la diferencia porcentual respecto a otra cotización.
    /// </summary>
    public decimal DiferenciaPorcentualRespectoA(Cotizacion otra)
    {
        if (otra is null || otra.MontoTotal.Valor == 0)
            return 0;

        return Math.Round(
            ((MontoTotal.Valor - otra.MontoTotal.Valor) / otra.MontoTotal.Valor) * 100,
            2,
            MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// Determina si esta cotización es más económica que otra.
    /// </summary>
    public bool EsMasEconomicaQue(Cotizacion otra) =>
        MontoTotal.Valor < otra.MontoTotal.Valor;

    #endregion

    #region Presentación

    /// <summary>
    /// Devuelve un resumen corto de la cotización para mostrar en UI.
    /// </summary>
    public string ResumenCorto() =>
        $"{NumeroCotizacion} - {MontoTotal} (Vence: {FechaVigencia:dd/MM/yyyy})";

    public override string ToString() =>
        $"Cotización {NumeroCotizacion} del Proveedor {ProveedorId} - {MontoTotal}";

    #endregion
}

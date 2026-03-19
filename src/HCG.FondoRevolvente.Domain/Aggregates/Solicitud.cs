using HCG.FondoRevolvente.Domain.Enums;
using HCG.FondoRevolvente.Domain.Exceptions;
using HCG.FondoRevolvente.Domain.ValueObjects;
using HCG.FondoRevolvente.Domain.Constants;

namespace HCG.FondoRevolvente.Domain.Aggregates;

/// <summary>
/// Aggregate Root - Representa una solicitud de adquisición por Fondo Revolvente.
/// Orquesta las reglas de negocio y transiciones de estado.
/// §Módulo 05 y §Módulo 06 README.
/// </summary>
public class Solicitud
{
    public int Id { get; private set; }

    /// <summary>Identificador único canónico (ej: DSA-2026-001).</summary>
    public FolioDSA Folio { get; private set; } = null!;

    /// <summary>Estado actual de la solicitud.</summary>
    public EstadoSolicitud Estado { get; private set; }

    /// <summary>Fase actual del proceso operativo.</summary>
    public FaseProceso Fase { get; private set; }

    /// <summary>Monto total de la solicitud (validado por RN-001).</summary>
    public MontoFondoRevolvente Monto { get; private set; } = null!;

    /// <summary>Descripción detallada de la necesidad o bien a adquirir.</summary>
    public string Descripcion { get; private set; } = null!;

    /// <summary>Usuario responsable de la captura inicial.</summary>
    public string Solicitante { get; private set; } = null!;

    /// <summary>Fecha de creación inicial.</summary>
    public DateTime FechaCreacion { get; private set; }

    /// <summary>Colección de cotizaciones asociadas (RN-003).</summary>
    private readonly List<Cotizacion> _cotizaciones = new();
    public IReadOnlyCollection<Cotizacion> Cotizaciones => _cotizaciones.AsReadOnly();

    /// <summary>Historial de hitos registrados (timeline).</summary>
    private readonly List<Hito> _historial = new();
    public IReadOnlyCollection<Hito> Historial => _historial.AsReadOnly();

    private Solicitud() { } // Para EF Core

    /// <summary>
    /// Crea una nueva solicitud en estado Recepcionado.
    /// </summary>
    public Solicitud(FolioDSA folio, MontoFondoRevolvente monto, string descripcion, string solicitante)
    {
        Folio = folio;
        Monto = monto;
        Descripcion = descripcion;
        Solicitante = solicitante;
        Estado = EstadoSolicitud.Recepcionado;
        Fase = FaseProceso.RecepcionValidacionInicial;
        FechaCreacion = DateTime.UtcNow;

        RegistrarHito(TipoHito.RecepcionOficio, "Solicitud recepcionada e iniciada", solicitante);
        RegistrarHito(TipoHito.AsignacionFolio, $"Folio asignado: {folio}", solicitante);
    }

    /// <summary>
    /// Agrega una cotización a la solicitud.
    /// </summary>
    public void AgregarCotizacion(int proveedorId, MontoFondoRevolvente monto, string archivoUri, string usuario)
    {
        if (Estado != EstadoSolicitud.SinCotizaciones && Estado != EstadoSolicitud.EnCotizacion)
            throw new DomainException($"No se pueden agregar cotizaciones en el estado actual: {Estado}");

        var cotizacion = new Cotizacion(proveedorId, Id, monto, archivoUri);
        _cotizaciones.Add(cotizacion);

        Fase = FaseProceso.EstudioMercadoCotizacion;
        RegistrarHito(TipoHito.RecepcionCotizacionProveedor, $"Cotización agregada por {monto}", usuario, archivoUri);
    }

    /// <summary>
    /// Selecciona el proveedor ganador basándose en una cotización.
    /// Aplica RN-003: mínimo de cotizaciones requeridas.
    /// </summary>
    public void SeleccionarProveedor(int cotizacionId, string usuario)
    {
        if (_cotizaciones.Count < LimitesNegocio.CotizacionesMinimasRequeridas)
            throw new CotizacionesInsuficientesException(_cotizaciones.Count, LimitesNegocio.CotizacionesMinimasRequeridas);

        foreach (var cot in _cotizaciones)
        {
            if (cot.Id == cotizacionId)
                cot.MarcarComoGanadora();
            else
                cot.DesmarcarComoGanadora();
        }

        RegistrarHito(TipoHito.SeleccionProveedor, "Proveedor ganador seleccionado", usuario);
    }

    /// <summary>
    /// Cambia el estado de la solicitud validando la transición.
    /// </summary>
    public void CambiarEstado(EstadoSolicitud nuevoEstado, TipoHito hito, string usuario, string? motivo = null)
    {
        var mensaje = $"Cambio de estado: {Estado} -> {nuevoEstado}";
        if (!string.IsNullOrEmpty(motivo)) mensaje += $". Motivo: {motivo}";

        Estado = nuevoEstado;
        Fase = nuevoEstado.ObtenerFase(); 
        RegistrarHito(hito, mensaje, usuario);
    }

    private void RegistrarHito(TipoHito tipo, string mensaje, string usuario, string? adjunto = null)
    {
        _historial.Add(new Hito(Id, tipo, mensaje, usuario, adjunto));
    }
}

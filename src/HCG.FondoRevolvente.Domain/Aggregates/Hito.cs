using HCG.FondoRevolvente.Domain.Enums;
using HCG.FondoRevolvente.Domain.Exceptions;

namespace HCG.FondoRevolvente.Domain.Aggregates;

/// <summary>
/// Representa un hito o evento significativo en el ciclo de vida de una solicitud.
/// Cada hito registra un avance en el proceso y contribuye al timeline visual.
/// Los 27 tipos de hito están distribuidos en las 8 fases del proceso.
/// </summary>
public class Hito
{
    #region Propiedades de Identidad

    /// <summary>
    /// Identificador único del hito.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// ID de la solicitud a la que pertenece este hito.
    /// </summary>
    public int SolicitudId { get; private set; }

    /// <summary>
    /// Referencia a la solicitud (navegación).
    /// </summary>
    public Solicitud? Solicitud { get; private set; }

    #endregion

    #region Propiedades del Hito

    /// <summary>
    /// Tipo de hito que identifica el evento registrado.
    /// </summary>
    public TipoHito Tipo { get; private set; }

    /// <summary>
    /// Estado de la solicitud en el momento del hito.
    /// </summary>
    public EstadoSolicitud EstadoEnElMomento { get; private set; }

    /// <summary>
    /// Fase del proceso en la que ocurrió el hito.
    /// </summary>
    public FaseProceso Fase { get; private set; }

    /// <summary>
    /// Fecha y hora en que ocurrió el hito.
    /// </summary>
    public DateTime FechaHora { get; private set; }

    /// <summary>
    /// Usuario que registró o provocó el hito.
    /// </summary>
    public string? Usuario { get; private set; }

    /// <summary>
    /// Nombre completo del usuario (desde Active Directory).
    /// </summary>
    public string? UsuarioNombreCompleto { get; private set; }

    /// <summary>
    /// Rol del usuario en el momento del hito.
    /// </summary>
    public RolAplicacion? UsuarioRol { get; private set; }

    /// <summary>
    /// Descripción o comentario adicional sobre el hito.
    /// </summary>
    public string? Comentario { get; private set; }

    /// <summary>
    /// Datos adicionales en formato JSON para información contextual.
    /// </summary>
    public string? DatosAdicionales { get; private set; }

    #endregion

    #region Propiedades de Referencias Externas

    /// <summary>
    /// ID de un proveedor relacionado con este hito (ej: proveedor seleccionado).
    /// </summary>
    public int? ProveedorId { get; private set; }

    /// <summary>
    /// Referencia al proveedor (navegación).
    /// </summary>
    public Proveedor? Proveedor { get; private set; }

    /// <summary>
    /// ID de una cotización relacionada con este hito.
    /// </summary>
    public int? CotizacionId { get; private set; }

    /// <summary>
    /// Referencia a la cotización (navegación).
    /// </summary>
    public Cotizacion? Cotizacion { get; private set; }

    #endregion

    #region Constructores

    // Constructor para EF Core
    private Hito() { }

    /// <summary>
    /// Crea un nuevo hito para una solicitud.
    /// </summary>
    public Hito(
        int solicitudId,
        TipoHito tipo,
        EstadoSolicitud estadoEnElMomento,
        string? usuario = null,
        string? usuarioNombreCompleto = null,
        RolAplicacion? usuarioRol = null,
        string? comentario = null)
    {
        if (!Enum.IsDefined(tipo))
            throw new DomainException($"El tipo de hito '{tipo}' no es válido.");

        SolicitudId = solicitudId;
        Tipo = tipo;
        EstadoEnElMomento = estadoEnElMomento;
        Fase = tipo.ObtenerFase();
        FechaHora = DateTime.UtcNow;
        Usuario = usuario;
        UsuarioNombreCompleto = usuarioNombreCompleto;
        UsuarioRol = usuarioRol;
        Comentario = comentario?.Trim();
    }

    #endregion

    #region Métodos de Dominio

    /// <summary>
    /// Establece la referencia a un proveedor relacionado con el hito.
    /// </summary>
    public Hito ConProveedor(int proveedorId)
    {
        ProveedorId = proveedorId;
        return this;
    }

    /// <summary>
    /// Establece la referencia a una cotización relacionada con el hito.
    /// </summary>
    public Hito ConCotizacion(int cotizacionId)
    {
        CotizacionId = cotizacionId;
        return this;
    }

    /// <summary>
    /// Establece datos adicionales en formato diccionario (se serializa a JSON).
    /// </summary>
    public Hito ConDatosAdicionales(Dictionary<string, object> datos)
    {
        if (datos is null || datos.Count == 0)
            return this;

        DatosAdicionales = System.Text.Json.JsonSerializer.Serialize(datos);
        return this;
    }

    /// <summary>
    /// Agrega un comentario al hito.
    /// </summary>
    public void AgregarComentario(string comentario)
    {
        if (!string.IsNullOrWhiteSpace(comentario))
        {
            Comentario = string.IsNullOrWhiteSpace(Comentario)
                ? comentario.Trim()
                : $"{Comentario}\n{comentario.Trim()}";
        }
    }

    /// <summary>
    /// Obtiene los datos adicionales deserializados.
    /// </summary>
    public Dictionary<string, object>? ObtenerDatosAdicionales()
    {
        if (string.IsNullOrWhiteSpace(DatosAdicionales))
            return null;

        try
        {
            return System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(DatosAdicionales);
        }
        catch
        {
            return null;
        }
    }

    #endregion

    #region Propiedades Calculadas

    /// <summary>
    /// Determina si este hito es un evento positivo (avance del proceso).
    /// </summary>
    public bool EsPositivo => Tipo.EsEventoNegativo() == false;

    /// <summary>
    /// Obtiene el nombre descriptivo del tipo de hito.
    /// </summary>
    public string NombreDescriptivo => Tipo.ToString();

    /// <summary>
    /// Obtiene el ícono de Segoe Fluent Icons para el hito.
    /// </summary>
    public string IconoFluent => Tipo.ObtenerIcono();

    /// <summary>
    /// Tiempo transcurrido desde el hito hasta ahora.
    /// </summary>
    public TimeSpan TiempoTranscurrido => DateTime.UtcNow - FechaHora;

    /// <summary>
    /// Descripción del tiempo transcurrido en formato legible.
    /// </summary>
    public string TiempoTranscurridoDescripcion
    {
        get
        {
            var transcurrido = TiempoTranscurrido;

            if (transcurrido.TotalMinutes < 1)
                return "Hace menos de un minuto";

            if (transcurrido.TotalHours < 1)
                return $"Hace {(int)transcurrido.TotalMinutes} minuto(s)";

            if (transcurrido.TotalDays < 1)
                return $"Hace {(int)transcurrido.TotalHours} hora(s)";

            if (transcurrido.TotalDays < 30)
                return $"Hace {(int)transcurrido.TotalDays} día(s)";

            return $"Hace más de {(int)(transcurrido.TotalDays / 30)} mes(es)";
        }
    }

    #endregion

    #region Factory Methods Estáticos

    /// <summary>
    /// Crea un hito de solicitud creada.
    /// </summary>
    public static Hito SolicitudCreada(int solicitudId, string? usuario, string? nombreCompleto, RolAplicacion? rol)
        => new Hito(solicitudId, TipoHito.RecepcionOficio, EstadoSolicitud.Recepcionado, usuario, nombreCompleto, rol);

    /// <summary>
    /// Crea un hito de solicitud enviada a cotización.
    /// </summary>
    public static Hito EnviadoACotizacion(int solicitudId, string? usuario, string? nombreCompleto, RolAplicacion? rol)
        => new Hito(solicitudId, TipoHito.SolicitudCotizaciones, EstadoSolicitud.EnCotizacion, usuario, nombreCompleto, rol);

    /// <summary>
    /// Crea un hito de cotización recibida.
    /// </summary>
    public static Hito CotizacionRecibida(int solicitudId, int proveedorId, int cotizacionId, string? usuario)
        => new Hito(solicitudId, TipoHito.RecepcionCotizacionProveedor, EstadoSolicitud.EnCotizacion, usuario)
            .ConProveedor(proveedorId)
            .ConCotizacion(cotizacionId);

    /// <summary>
    /// Crea un hito de proveedor seleccionado.
    /// </summary>
    public static Hito ProveedorSeleccionado(int solicitudId, int proveedorId, int cotizacionId, string? usuario)
        => new Hito(solicitudId, TipoHito.SeleccionProveedor, EstadoSolicitud.ProveedorSeleccionado, usuario)
            .ConProveedor(proveedorId)
            .ConCotizacion(cotizacionId);

    /// <summary>
    /// Crea un hito de autorización CAA.
    /// </summary>
    public static Hito AutorizadoPorCaa(int solicitudId, string? usuario, string? nombreCompleto, string? comentario)
        => new Hito(solicitudId, TipoHito.ResolucionCAA, EstadoSolicitud.AutorizadoCAA, usuario, nombreCompleto, RolAplicacion.RevisorCAA, comentario);

    /// <summary>
    /// Crea un hito de rechazo CAA.
    /// </summary>
    public static Hito RechazadoPorCaa(int solicitudId, string? usuario, string? nombreCompleto, string? razon)
        => new Hito(solicitudId, TipoHito.ResolucionCAA, EstadoSolicitud.RechazadoCAA, usuario, nombreCompleto, RolAplicacion.RevisorCAA, razon);

    /// <summary>
    /// Crea un hito de CFDI validado.
    /// </summary>
    public static Hito CfdiValidadoSat(int solicitudId, string uuidCfdi, string? usuario)
        => new Hito(solicitudId, TipoHito.ConfirmacionCFDIValido, EstadoSolicitud.CfdiValido, usuario, null, RolAplicacion.Finanzas)
            .ConDatosAdicionales(new Dictionary<string, object> { ["UuidCfdi"] = uuidCfdi });

    /// <summary>
    /// Crea un hito de pago realizado.
    /// </summary>
    public static Hito PagoRealizado(int solicitudId, decimal monto, string? usuario)
        => new Hito(solicitudId, TipoHito.ConfirmacionPagoEjecutado, EstadoSolicitud.Pagado, usuario, null, RolAplicacion.Finanzas)
            .ConDatosAdicionales(new Dictionary<string, object> { ["Monto"] = monto });

    /// <summary>
    /// Crea un hito de entrega confirmada.
    /// </summary>
    public static Hito EntregaConfirmada(int solicitudId, string? usuario, string? nombreCompleto)
        => new Hito(solicitudId, TipoHito.RecepcionAlmacenHCG, EstadoSolicitud.RecepcionadoBienes, usuario, nombreCompleto, RolAplicacion.Almacen);

    /// <summary>
    /// Crea un hito de solicitud cerrada.
    /// </summary>
    public static Hito SolicitudCerrada(int solicitudId, string? usuario, string? nombreCompleto)
        => new Hito(solicitudId, TipoHito.CierreOficialExpediente, EstadoSolicitud.Cerrado, usuario, nombreCompleto);

    /// <summary>
    /// Crea un hito de expediente archivado.
    /// </summary>
    public static Hito ExpedienteArchivado(int solicitudId, string rutaSmb)
        => new Hito(solicitudId, TipoHito.CierreOficialExpediente, EstadoSolicitud.Cerrado)
            .ConDatosAdicionales(new Dictionary<string, object> { ["RutaSmb"] = rutaSmb });

    #endregion

    #region Presentación

    public override string ToString() =>
        $"[{FechaHora:dd/MM/yyyy HH:mm}] {Tipo.ObtenerFase()} - {Comentario ?? "Sin comentario"}";

    #endregion
}

namespace HCG.FondoRevolvente.Domain.Events;

/// <summary>
/// Evento emitido cuando una nueva solicitud es creada.
/// </summary>
public sealed record SolicitudCreadaEvent : DomainEventBase
{
    /// <summary>
    /// ID de la solicitud creada.
    /// </summary>
    public int SolicitudId { get; }

    /// <summary>
    /// Folio de la solicitud.
    /// </summary>
    public string Folio { get; }

    /// <summary>
    /// Usuario que creó la solicitud.
    /// </summary>
    public string? UsuarioCreacion { get; }

    public SolicitudCreadaEvent(int solicitudId, string folio, string? usuarioCreacion)
    {
        SolicitudId = solicitudId;
        Folio = folio;
        UsuarioCreacion = usuarioCreacion;
    }
}

/// <summary>
/// Evento emitido cuando una solicitud es enviada a cotización.
/// </summary>
public sealed record SolicitudEnviadaCotizacionEvent : DomainEventBase
{
    public int SolicitudId { get; }
    public string Folio { get; }
    public string? Usuario { get; }

    public SolicitudEnviadaCotizacionEvent(int solicitudId, string folio, string? usuario)
    {
        SolicitudId = solicitudId;
        Folio = folio;
        Usuario = usuario;
    }
}

/// <summary>
/// Evento emitido cuando un proveedor es seleccionado como ganador.
/// </summary>
public sealed record ProveedorSeleccionadoEvent : DomainEventBase
{
    public int SolicitudId { get; }
    public string Folio { get; }
    public int ProveedorId { get; }
    public int CotizacionId { get; }

    public ProveedorSeleccionadoEvent(int solicitudId, string folio, int proveedorId, int cotizacionId)
    {
        SolicitudId = solicitudId;
        Folio = folio;
        ProveedorId = proveedorId;
        CotizacionId = cotizacionId;
    }
}

/// <summary>
/// Evento emitido cuando una solicitud es autorizada por el CAA.
/// </summary>
public sealed record SolicitudAutorizadaCaaEvent : DomainEventBase
{
    public int SolicitudId { get; }
    public string Folio { get; }
    public string? UsuarioAutorizacion { get; }

    public SolicitudAutorizadaCaaEvent(int solicitudId, string folio, string? usuarioAutorizacion)
    {
        SolicitudId = solicitudId;
        Folio = folio;
        UsuarioAutorizacion = usuarioAutorizacion;
    }
}

/// <summary>
/// Evento emitido cuando una solicitud es rechazada por el CAA.
/// </summary>
public sealed record SolicitudRechazadaCaaEvent : DomainEventBase
{
    public int SolicitudId { get; }
    public string Folio { get; }
    public string Motivo { get; }
    public string? Usuario { get; }

    public SolicitudRechazadaCaaEvent(int solicitudId, string folio, string motivo, string? usuario)
    {
        SolicitudId = solicitudId;
        Folio = folio;
        Motivo = motivo;
        Usuario = usuario;
    }
}

/// <summary>
/// Evento emitido cuando un CFDI es validado exitosamente.
/// </summary>
public sealed record CfdiValidadoEvent : DomainEventBase
{
    public int SolicitudId { get; }
    public string Folio { get; }
    public string UuidCfdi { get; }

    public CfdiValidadoEvent(int solicitudId, string folio, string uuidCfdi)
    {
        SolicitudId = solicitudId;
        Folio = folio;
        UuidCfdi = uuidCfdi;
    }
}

/// <summary>
/// Evento emitido cuando un pago es realizado.
/// </summary>
public sealed record PagoRealizadoEvent : DomainEventBase
{
    public int SolicitudId { get; }
    public string Folio { get; }
    public decimal Monto { get; }

    public PagoRealizadoEvent(int solicitudId, string folio, decimal monto)
    {
        SolicitudId = solicitudId;
        Folio = folio;
        Monto = monto;
    }
}

/// <summary>
/// Evento emitido cuando una entrega es confirmada.
/// </summary>
public sealed record EntregaConfirmadaEvent : DomainEventBase
{
    public int SolicitudId { get; }
    public string Folio { get; }

    public EntregaConfirmadaEvent(int solicitudId, string folio)
    {
        SolicitudId = solicitudId;
        Folio = folio;
    }
}

/// <summary>
/// Evento emitido cuando una solicitud es cerrada.
/// </summary>
public sealed record SolicitudCerradaEvent : DomainEventBase
{
    public int SolicitudId { get; }
    public string Folio { get; }

    public SolicitudCerradaEvent(int solicitudId, string folio)
    {
        SolicitudId = solicitudId;
        Folio = folio;
    }
}

/// <summary>
/// Evento emitido cuando una solicitud es cancelada.
/// </summary>
public sealed record SolicitudCanceladaEvent : DomainEventBase
{
    public int SolicitudId { get; }
    public string Folio { get; }
    public string Motivo { get; }
    public string? Usuario { get; }

    public SolicitudCanceladaEvent(int solicitudId, string folio, string motivo, string? usuario)
    {
        SolicitudId = solicitudId;
        Folio = folio;
        Motivo = motivo;
        Usuario = usuario;
    }
}

/// <summary>
/// Evento emitido cuando el estado de una solicitud cambia.
/// Evento genérico para tracking de todos los cambios de estado.
/// </summary>
public sealed record EstadoCambiadoEvent : DomainEventBase
{
    public int SolicitudId { get; }
    public string Folio { get; }
    public string EstadoAnterior { get; }
    public string EstadoNuevo { get; }
    public string? Usuario { get; }
    public string? Comentario { get; }

    public EstadoCambiadoEvent(
        int solicitudId,
        string folio,
        string estadoAnterior,
        string estadoNuevo,
        string? usuario,
        string? comentario = null)
    {
        SolicitudId = solicitudId;
        Folio = folio;
        EstadoAnterior = estadoAnterior;
        EstadoNuevo = estadoNuevo;
        Usuario = usuario;
        Comentario = comentario;
    }
}

using HCG.FondoRevolvente.Domain.Enums;

namespace HCG.FondoRevolvente.Domain.Aggregates;

/// <summary>
/// Registro inmutable de un evento relevante en el ciclo de vida de la solicitud.
/// §Módulo 07 README.
/// </summary>
public class Hito
{
    public int Id { get; private set; }

    /// <summary>Referencia a la solicitud a la que pertenece.</summary>
    public int SolicitudId { get; private set; }

    /// <summary>Fecha y hora exacta del evento (UTC).</summary>
    public DateTime FechaUtc { get; private set; }

    /// <summary>Enumeración que clasifica el tipo de evento.</summary>
    public TipoHito Tipo { get; private set; }

    /// <summary>Descripción detallada del evento.</summary>
    public string Mensaje { get; private set; }

    /// <summary>Usuario que originó el evento.</summary>
    public string Usuario { get; private set; }

    /// <summary>Referencia opcional a un archivo relacionado (ej: PDF de CFDI).</summary>
    public string? ReferenciaAdjunto { get; private set; }

    private Hito() { } // Para EF Core

    public Hito(int solicitudId, TipoHito tipo, string mensaje, string usuario, string? referenciaAdjunto = null)
    {
        SolicitudId = solicitudId;
        FechaUtc = DateTime.UtcNow;
        Tipo = tipo;
        Mensaje = mensaje;
        Usuario = usuario;
        ReferenciaAdjunto = referenciaAdjunto;
    }
}

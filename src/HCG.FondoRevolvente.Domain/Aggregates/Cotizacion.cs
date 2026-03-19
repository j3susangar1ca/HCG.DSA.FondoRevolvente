using HCG.FondoRevolvente.Domain.ValueObjects;

namespace HCG.FondoRevolvente.Domain.Aggregates;

/// <summary>
/// Representa una propuesta comercial enviada por un proveedor para una solicitud.
/// §Módulo 09 README.
/// </summary>
public class Cotizacion
{
    public int Id { get; private set; }

    /// <summary>Referencia al proveedor que emite la cotización.</summary>
    public int ProveedorId { get; private set; }

    /// <summary>Referencia a la solicitud a la que pertenece.</summary>
    public int SolicitudId { get; private set; }

    /// <summary>Monto total de la cotización, validado bajo la RN-001.</summary>
    public MontoFondoRevolvente Monto { get; private set; }

    /// <summary>Ruta o identificador del archivo PDF en el almacenamiento SMB.</summary>
    public string ArchivoAdjuntoUri { get; private set; }

    /// <summary>Fecha en que se cargó la cotización al sistema.</summary>
    public DateTime FechaCarga { get; private set; }

    /// <summary>Indica si esta cotización fue la seleccionada para la compra definitiva.</summary>
    public bool EsGanadora { get; private set; }

    private Cotizacion() { } // Para EF Core

    public Cotizacion(int proveedorId, int solicitudId, MontoFondoRevolvente monto, string archivoAdjuntoUri)
    {
        ProveedorId = proveedorId;
        SolicitudId = solicitudId;
        Monto = monto;
        ArchivoAdjuntoUri = archivoAdjuntoUri;
        FechaCarga = DateTime.UtcNow;
        EsGanadora = false;
    }

    public void MarcarComoGanadora()
    {
        EsGanadora = true;
    }

    public void DesmarcarComoGanadora()
    {
        EsGanadora = false;
    }
}

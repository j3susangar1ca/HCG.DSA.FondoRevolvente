using HCG.FondoRevolvente.Domain.ValueObjects;

namespace HCG.FondoRevolvente.Domain.Aggregates;

/// <summary>
/// Representa un proveedor registrado en el HCG para operaciones de adquisición.
/// §Módulo 08 README.
/// </summary>
public class Proveedor
{
    public int Id { get; private set; }

    /// <summary>Nombre comercial o razón social del proveedor.</summary>
    public string RazonSocial { get; private set; }

    /// <summary>RFC validado estructuralmente y enmascarado según rol.</summary>
    public RfcProveedor Rfc { get; private set; }

    /// <summary>Indica si el proveedor está habilitado para recibir nuevas solicitudes.</summary>
    public bool EstaActivo { get; private set; }

    /// <summary>Fecha del último registro de actividad del proveedor.</summary>
    public DateTime UltimaActividad { get; private set; }

    private Proveedor() { } // Para EF Core

    public Proveedor(string razonSocial, RfcProveedor rfc)
    {
        RazonSocial = razonSocial;
        Rfc = rfc;
        EstaActivo = true;
        UltimaActividad = DateTime.UtcNow;
    }

    public void Desactivar()
    {
        EstaActivo = false;
        UltimaActividad = DateTime.UtcNow;
    }

    public void Activar()
    {
        EstaActivo = true;
        UltimaActividad = DateTime.UtcNow;
    }

    public void ActualizarRazonSocial(string nuevaRazonSocial)
    {
        RazonSocial = nuevaRazonSocial;
        UltimaActividad = DateTime.UtcNow;
    }
}

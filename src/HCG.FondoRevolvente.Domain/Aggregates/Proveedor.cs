using HCG.FondoRevolvente.Domain.Exceptions;
using HCG.FondoRevolvente.Domain.ValueObjects;

namespace HCG.FondoRevolvente.Domain.Aggregates;

/// <summary>
/// Representa un proveedor registrado en el sistema de Fondo Revolvente.
/// Entidad que participa en el proceso de cotización y puede ser seleccionada
/// como proveedor ganador de una solicitud de adquisición.
/// </summary>
public class Proveedor
{
    #region Propiedades de Identidad

    /// <summary>
    /// Identificador único del proveedor.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// RFC del proveedor validado ante el SAT.
    /// </summary>
    public RfcProveedor Rfc { get; private set; } = null!;

    #endregion

    #region Propiedades de Información General

    /// <summary>
    /// Razón social o nombre comercial del proveedor.
    /// </summary>
    public string RazonSocial { get; private set; } = null!;

    /// <summary>
    /// Nombre comercial (puede ser diferente a la razón social).
    /// </summary>
    public string? NombreComercial { get; private set; }

    /// <summary>
    /// Email principal de contacto.
    /// </summary>
    public string Email { get; private set; } = null!;

    /// <summary>
    /// Teléfono principal de contacto.
    /// </summary>
    public string? Telefono { get; private set; }

    /// <summary>
    /// Dirección fiscal del proveedor.
    /// </summary>
    public string? DireccionFiscal { get; private set; }

    /// <summary>
    /// Código postal del proveedor.
    /// </summary>
    public string? CodigoPostal { get; private set; }

    #endregion

    #region Propiedades de Estado

    /// <summary>
    /// Indica si el proveedor está activo para participar en cotizaciones.
    /// </summary>
    public bool Activo { get; private set; } = true;

    /// <summary>
    /// Fecha de registro del proveedor en el sistema.
    /// </summary>
    public DateTime FechaRegistro { get; private set; }

    /// <summary>
    /// Fecha de última modificación de los datos del proveedor.
    /// </summary>
    public DateTime? FechaUltimaModificacion { get; private set; }

    /// <summary>
    /// Usuario que registró al proveedor.
    /// </summary>
    public string? UsuarioRegistro { get; private set; }

    /// <summary>
    /// Notas o comentarios sobre el proveedor.
    /// </summary>
    public string? Notas { get; private set; }

    #endregion

    #region Propiedades de Validación SAT

    /// <summary>
    /// Indica si el RFC ha sido validado exitosamente ante el SAT.
    /// </summary>
    public bool RfcValidadoSat { get; private set; }

    /// <summary>
    /// Fecha de la última validación del RFC ante el SAT.
    /// </summary>
    public DateTime? FechaUltimaValidacionSat { get; private set; }

    #endregion

    #region Propiedades de Navegación

    /// <summary>
    /// Colección de cotizaciones enviadas por este proveedor.
    /// </summary>
    private readonly List<Cotizacion> _cotizaciones = [];
    public IReadOnlyCollection<Cotizacion> Cotizaciones => _cotizaciones.AsReadOnly();

    #endregion

    #region Constructores

    // Constructor para EF Core
    private Proveedor() { }

    /// <summary>
    /// Crea una nueva instancia de proveedor con los datos mínimos requeridos.
    /// </summary>
    public Proveedor(
        RfcProveedor rfc,
        string razonSocial,
        string email,
        string? usuarioRegistro = null)
    {
        ValidarDatosBasicos(razonSocial, email);

        Rfc = rfc ?? throw new DomainException("El RFC del proveedor es requerido.");
        RazonSocial = razonSocial.Trim();
        Email = email.Trim().ToLowerInvariant();
        FechaRegistro = DateTime.UtcNow;
        UsuarioRegistro = usuarioRegistro;
        Activo = true;
    }

    #endregion

    #region Métodos de Dominio

    /// <summary>
    /// Actualiza los datos generales del proveedor.
    /// </summary>
    public void ActualizarDatos(
        string razonSocial,
        string? nombreComercial,
        string email,
        string? telefono,
        string? direccionFiscal,
        string? codigoPostal,
        string? notas)
    {
        ValidarDatosBasicos(razonSocial, email);

        RazonSocial = razonSocial.Trim();
        NombreComercial = nombreComercial?.Trim();
        Email = email.Trim().ToLowerInvariant();
        Telefono = telefono?.Trim();
        DireccionFiscal = direccionFiscal?.Trim();
        CodigoPostal = codigoPostal?.Trim();
        Notas = notas?.Trim();
        FechaUltimaModificacion = DateTime.UtcNow;
    }

    /// <summary>
    /// Activa el proveedor para que pueda participar en cotizaciones.
    /// </summary>
    public void Activar()
    {
        Activo = true;
        FechaUltimaModificacion = DateTime.UtcNow;
    }

    /// <summary>
    /// Desactiva el proveedor. No será considerado en nuevas cotizaciones.
    /// </summary>
    public void Desactivar(string? razon = null)
    {
        Activo = false;
        FechaUltimaModificacion = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(razon))
        {
            Notas = string.IsNullOrWhiteSpace(Notas)
                ? $"[Desactivado: {razon}]"
                : $"{Notas}\n[Desactivado: {razon}]";
        }
    }

    /// <summary>
    /// Registra el resultado de la validación del RFC ante el SAT.
    /// </summary>
    public void RegistrarValidacionSat(bool exitoso)
    {
        RfcValidadoSat = exitoso;
        FechaUltimaValidacionSat = DateTime.UtcNow;
        FechaUltimaModificacion = DateTime.UtcNow;
    }

    /// <summary>
    /// Agrega una cotización a la historia del proveedor.
    /// </summary>
    internal void AgregarCotizacion(Cotizacion cotizacion)
    {
        if (cotizacion is null)
            throw new DomainException("La cotización no puede ser nula.");

        _cotizaciones.Add(cotizacion);
    }

    /// <summary>
    /// Obtiene el número total de cotizaciones enviadas por este proveedor.
    /// </summary>
    public int TotalCotizacionesEnviadas => _cotizaciones.Count;

    /// <summary>
    /// Obtiene el número de cotizaciones ganadas por este proveedor.
    /// </summary>
    public int CotizacionesGanadas => _cotizaciones.Count(c => c.EsGanadora);

    #endregion

    #region Validaciones Privadas

    private static void ValidarDatosBasicos(string razonSocial, string email)
    {
        if (string.IsNullOrWhiteSpace(razonSocial))
            throw new DomainException("La razón social del proveedor es requerida.");

        if (razonSocial.Length > 250)
            throw new DomainException("La razón social no puede exceder 250 caracteres.");

        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("El email del proveedor es requerido.");

        if (!IsValidEmail(email))
            throw new DomainException($"El email '{email}' no tiene un formato válido.");
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email.ToLowerInvariant().Trim();
        }
        catch
        {
            return false;
        }
    }

    #endregion

    #region Presentación

    /// <summary>
    /// Devuelve el nombre a mostrar en la UI (preferentemente nombre comercial).
    /// </summary>
    public string NombreParaMostrar() =>
        !string.IsNullOrWhiteSpace(NombreComercial) ? NombreComercial : RazonSocial;

    public override string ToString() => $"{Rfc} - {RazonSocial}";

    #endregion
}

using System.Text.RegularExpressions;
using HCG.FondoRevolvente.Domain.Exceptions;

namespace HCG.FondoRevolvente.Domain.ValueObjects;

/// <summary>
/// Encapsula el folio único de identificación de una solicitud de adquisición.
/// Formato canónico: DSA-YYYY-NNN (ej: DSA-2026-089).
/// Generado por el sistema al crear la solicitud — nunca capturado manualmente.
/// </summary>
public sealed record FolioDSA
{
    // Regex compilada en tiempo de carga — rendimiento O(1) en validaciones repetidas
    private static readonly Regex _formatoValido =
        new(@"^DSA-(\d{4})-(\d{3,6})$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    /// <summary>Ejercicio fiscal al que pertenece la solicitud (YYYY).</summary>
    public int EjercicioFiscal { get; }

    /// <summary>Número secuencial de la solicitud dentro del ejercicio (NNN).</summary>
    public int NumeroSecuencial { get; }

    /// <summary>Folio completo en formato canónico: DSA-YYYY-NNN.</summary>
    public string Valor { get; }

    private FolioDSA(int ejercicioFiscal, int numeroSecuencial)
    {
        EjercicioFiscal = ejercicioFiscal;
        NumeroSecuencial = numeroSecuencial;
        Valor = Formatear(ejercicioFiscal, numeroSecuencial);
    }

    // -------------------------------------------------------------------------
    // Factory Methods
    // -------------------------------------------------------------------------

    /// <summary>
    /// Genera el folio para una nueva solicitud.
    /// Invocado por el sistema — nunca por el usuario directamente.
    /// </summary>
    /// <param name="ejercicioFiscal">Año del ejercicio fiscal (ej: 2026).</param>
    /// <param name="siguienteSecuencial">Número correlativo obtenido de la base de datos.</param>
    public static FolioDSA Generar(int ejercicioFiscal, int siguienteSecuencial)
    {
        ValidarEjercicioFiscal(ejercicioFiscal);
        ValidarSecuencial(siguienteSecuencial);

        return new FolioDSA(ejercicioFiscal, siguienteSecuencial);
    }

    /// <summary>
    /// Reconstituye un FolioDSA desde su representación en cadena (lectura desde BD).
    /// </summary>
    /// <param name="folio">Cadena con formato DSA-YYYY-NNN.</param>
    /// <exception cref="DomainException">Si el formato es inválido.</exception>
    public static FolioDSA Desde(string folio)
    {
        if (string.IsNullOrWhiteSpace(folio))
            throw new DomainException("El folio DSA no puede estar vacío.");

        var match = _formatoValido.Match(folio.Trim().ToUpperInvariant());

        if (!match.Success)
            throw new DomainException(
                $"Formato de folio inválido: '{folio}'. " +
                 "Se esperaba el formato DSA-YYYY-NNN (ejemplo: DSA-2026-001).");

        var ejercicio = int.Parse(match.Groups[1].Value);
        var secuencial = int.Parse(match.Groups[2].Value);

        ValidarEjercicioFiscal(ejercicio);
        ValidarSecuencial(secuencial);

        return new FolioDSA(ejercicio, secuencial);
    }

    /// <summary>
    /// Valida el formato sin lanzar excepción.
    /// Útil para validaciones en tiempo real en el AutoSuggestBox del ViewModel.
    /// </summary>
    public static bool EsFormatoValido(string? folio) =>
        !string.IsNullOrWhiteSpace(folio) &&
        _formatoValido.IsMatch(folio.Trim().ToUpperInvariant());

    // -------------------------------------------------------------------------
    // Métodos de dominio
    // -------------------------------------------------------------------------

    /// <summary>
    /// Verifica si este folio pertenece al mismo ejercicio fiscal que otro.
    /// Usado por ValidadorFraccionamientoService (RN-002).
    /// </summary>
    public bool EsDelMismoEjercicio(FolioDSA otro) =>
        EjercicioFiscal == otro.EjercicioFiscal;

    // -------------------------------------------------------------------------
    // Helpers privados
    // -------------------------------------------------------------------------

    private static string Formatear(int ejercicio, int secuencial) =>
        $"DSA-{ejercicio:D4}-{secuencial:D3}";

    private static void ValidarEjercicioFiscal(int ejercicio)
    {
        var anioActual = DateTime.UtcNow.Year;
        // Permite el año en curso y el año anterior (correcciones de cierre de ejercicio)
        if (ejercicio < anioActual - 1 || ejercicio > anioActual + 1)
            throw new DomainException(
                $"Ejercicio fiscal '{ejercicio}' fuera del rango operativo permitido.");
    }

    private static void ValidarSecuencial(int secuencial)
    {
        if (secuencial <= 0)
            throw new DomainException(
                $"El número secuencial debe ser mayor a cero. Recibido: {secuencial}.");
    }

    // -------------------------------------------------------------------------
    // Presentación
    // -------------------------------------------------------------------------

    /// <summary>Devuelve el folio en formato canónico: DSA-YYYY-NNN.</summary>
    public override string ToString() => Valor;

    /// <summary>
    /// Representación de "folio pendiente de asignación".
    /// Usada en el indicador del formulario de creación (§Módulo 05 README).
    /// </summary>
    public static string PendienteDeAsignacion => "DSA-\u2014\u2014\u2014\u2014-\u2014\u2014\u2014";
}

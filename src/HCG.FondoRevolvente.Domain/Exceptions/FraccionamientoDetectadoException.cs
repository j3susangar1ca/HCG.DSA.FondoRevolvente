using System.Linq;

namespace HCG.FondoRevolvente.Domain.Exceptions;

/// <summary>
/// Excepción lanzada cuando se detecta un posible fraccionamiento de operaciones.
/// Implementa RN-002: Detección de fraccionamiento para evadir el límite de $75,000 MXN.
/// </summary>
public class FraccionamientoDetectadoException : DomainException
{
    /// <summary>
    /// RFC del proveedor relacionado con las operaciones.
    /// </summary>
    public string RfcProveedor { get; }

    /// <summary>
    /// Suma total de las operaciones detectadas en el período.
    /// </summary>
    public decimal SumaOperaciones { get; }

    /// <summary>
    /// Número de operaciones detectadas en el período.
    /// </summary>
    public int NumeroOperaciones { get; }

    /// <summary>
    /// Período en días analizado para la detección.
    /// </summary>
    public int DiasPeriodo { get; }

    /// <summary>
    /// Folios de las solicitudes relacionadas.
    /// </summary>
    public IReadOnlyList<string> FoliosRelacionados { get; }

    public FraccionamientoDetectadoException(
        string rfcProveedor,
        decimal sumaOperaciones,
        int numeroOperaciones,
        int diasPeriodo,
        IEnumerable<string> foliosRelacionados)
        : base(
            "RN002_FRACCIONAMIENTO_DETECTADO",
            $"Se detectó posible fraccionamiento de operaciones. " +
            $"El proveedor con RFC {MaskRfc(rfcProveedor)} tiene {numeroOperaciones} solicitudes " +
            $"por un total de {sumaOperaciones:C2} MXN en los últimos {diasPeriodo} días. " +
            $"Esto representa {(sumaOperaciones / Constants.LimitesNegocio.MontoMaximoFondoRevolvente):P2} del límite normativo.")
    {
        RfcProveedor = rfcProveedor;
        SumaOperaciones = sumaOperaciones;
        NumeroOperaciones = numeroOperaciones;
        DiasPeriodo = diasPeriodo;
        FoliosRelacionados = foliosRelacionados.ToList().AsReadOnly();

        ConDatos("RfcProveedor", MaskRfc(rfcProveedor))
            .ConDatos("SumaOperaciones", sumaOperaciones)
            .ConDatos("NumeroOperaciones", numeroOperaciones)
            .ConDatos("DiasPeriodo", diasPeriodo)
            .ConDatos("FoliosRelacionados", string.Join(", ", FoliosRelacionados));
    }

    /// <summary>
    /// Enmascara el RFC para mostrar solo los primeros 4 caracteres y los últimos 3.
    /// Ejemplo: ABCD123456XXX -> ABCD****XXX
    /// </summary>
    private static string MaskRfc(string rfc) =>
        rfc.Length <= 7 ? rfc : $"{rfc[..4]}****{rfc[^3..]}";
}

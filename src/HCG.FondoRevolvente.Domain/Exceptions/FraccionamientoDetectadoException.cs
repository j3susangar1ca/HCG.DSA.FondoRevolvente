namespace HCG.FondoRevolvente.Domain.Exceptions;

/// <summary>
/// RN-002 — Se lanza cuando se detecta un posible fraccionamiento de compra.
/// </summary>
public sealed class FraccionamientoDetectadoException(string codigoProducto, string folioExistente)
    : DomainException(
        $"Posible fraccionamiento detectado (RN-002): el código '{codigoProducto}' " +
        $"fue adquirido en la solicitud {folioExistente} del mismo ejercicio fiscal.")
{
    public string CodigoProducto { get; } = codigoProducto;
    public string FolioExistente { get; } = folioExistente;
}

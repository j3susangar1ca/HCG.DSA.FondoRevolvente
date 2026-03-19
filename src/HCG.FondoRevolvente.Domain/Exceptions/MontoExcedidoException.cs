namespace HCG.FondoRevolvente.Domain.Exceptions;

/// <summary>
/// RN-001 — Se lanza cuando un monto excede el límite normativo del Fondo Revolvente.
/// La capa de presentación la captura para activar el InfoBar de advertencia
/// y deshabilitar el botón "Registrar Solicitud" (§Módulo 05 README).
/// </summary>
public sealed class MontoExcedidoException(decimal montoIntentado, decimal limiteMaximo)
    : DomainException(
        $"El monto ${montoIntentado:N2} MXN excede el límite máximo del Fondo Revolvente " +
        $"(${limiteMaximo:N2} MXN). Regla de Negocio RN-001.")
{
    public decimal MontoIntentado { get; } = montoIntentado;
    public decimal LimiteMaximo { get; } = limiteMaximo;
    public decimal Excedente { get; } = montoIntentado - limiteMaximo;
}

namespace HCG.FondoRevolvente.Domain.Exceptions;

/// <summary>
/// Excepción lanzada cuando un monto excede el límite máximo permitido.
/// Implementa RN-001: Límite máximo de $75,000.00 MXN por operación.
/// </summary>
public class MontoExcedidoException : DomainException
{
    /// <summary>
    /// Monto que intentó superarse el límite.
    /// </summary>
    public decimal MontoIntentado { get; }

    /// <summary>
    /// Límite máximo permitido.
    /// </summary>
    public decimal LimiteMaximo { get; }

    /// <summary>
    /// Porcentaje en que se excedió el límite.
    /// </summary>
    public double PorcentajeExceso { get; }

    public MontoExcedidoException(decimal monto, decimal limite)
        : base(
            "RN001_MONTO_EXCEDIDO",
            $"El monto {monto:C2} MXN excede el límite máximo permitido de {limite:C2} MXN " +
            $"para operaciones de Fondo Revolvente (RN-001, Art. 57 Ley de Compras del Estado de Jalisco).")
    {
        MontoIntentado = monto;
        LimiteMaximo = limite;
        PorcentajeExceso = (double)(monto / limite) - 1.0;

        ConDatos("MontoIntentado", monto)
            .ConDatos("LimiteMaximo", limite)
            .ConDatos("PorcentajeExceso", PorcentajeExceso);
    }

    public MontoExcedidoException(decimal monto, decimal limite, string contexto)
        : base(
            "RN001_MONTO_EXCEDIDO",
            $"El monto {monto:C2} MXN excede el límite máximo permitido de {limite:C2} MXN. " +
            $"Contexto: {contexto}")
    {
        MontoIntentado = monto;
        LimiteMaximo = limite;
        PorcentajeExceso = (double)(monto / limite) - 1.0;

        ConDatos("MontoIntentado", monto)
            .ConDatos("LimiteMaximo", limite)
            .ConDatos("PorcentajeExceso", PorcentajeExceso)
            .ConDatos("Contexto", contexto);
    }
}

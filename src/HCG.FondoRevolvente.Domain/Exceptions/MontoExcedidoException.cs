namespace HCG.FondoRevolvente.Domain.Exceptions;

public class MontoExcedidoException : DomainException
{
    public MontoExcedidoException(decimal monto, decimal limite)
        : base($"El monto {monto:C2} excede el límite máximo permitido de {limite:C2} para operaciones de Fondo Revolvente (RN-001).")
    {
    }
}

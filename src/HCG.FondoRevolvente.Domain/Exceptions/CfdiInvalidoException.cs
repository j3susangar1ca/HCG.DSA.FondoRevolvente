namespace HCG.FondoRevolvente.Domain.Exceptions;

/// <summary>RN-004 — CFDI rechazado o inválido según el servicio SAT.</summary>
public sealed class CfdiInvalidoException(string uuid, string razonSat)
    : DomainException($"CFDI con UUID '{uuid}' es inválido según el SAT: {razonSat}.")
{
    public string Uuid { get; } = uuid;
    public string RazonSat { get; } = razonSat;
}

using HCG.FondoRevolvente.Domain.ValueObjects;

namespace HCG.FondoRevolvente.Application.Interfaces;

public interface ISatValidationService
{
    Task<(bool IsValid, string Message)> ValidateUuidAsync(string uuid, RfcProveedor rfcEmisor, MontoFondoRevolvente monto, CancellationToken ct = default);
}

namespace HCG.FondoRevolvente.Domain.Exceptions;

/// <summary>RN-003 — Intento de selección de proveedor sin el mínimo de cotizaciones.</summary>
public sealed class CotizacionesInsuficientesException(int cotizacionesActuales, int minimoRequerido)
    : DomainException(
        $"Se requieren al menos {minimoRequerido} cotizaciones de proveedores distintos (RN-003). " +
        $"Cotizaciones actuales: {cotizacionesActuales}.")
{
    public int CotizacionesActuales { get; } = cotizacionesActuales;
    public int MinimoRequerido { get; } = minimoRequerido;
}

namespace HCG.FondoRevolvente.Domain.Exceptions;

/// <summary>Excepción base para todas las violaciones de invariantes del dominio.</summary>
public class DomainException(string message) : Exception(message);

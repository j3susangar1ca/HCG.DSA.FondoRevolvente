using HCG.FondoRevolvente.Domain.Constants;
using HCG.FondoRevolvente.Domain.Exceptions;

namespace HCG.FondoRevolvente.Domain.ValueObjects;

/// <summary>
/// Encapsula un monto monetario válido para el Fondo Revolvente del HCG.
/// Aplica RN-001: límite máximo de $75,000.00 MXN por operación
/// (Ley de Compras del Estado de Jalisco, Art. 57 del Reglamento).
/// </summary>
public sealed record MontoFondoRevolvente
{
    /// <summary>Valor monetario en pesos mexicanos, con hasta 2 decimales de precisión.</summary>
    public decimal Valor { get; }

    private MontoFondoRevolvente(decimal valor) => Valor = valor;

    // -------------------------------------------------------------------------
    // Factory Methods — única vía de construcción, garantiza invariantes
    // -------------------------------------------------------------------------

    /// <summary>
    /// Crea una instancia validada de <see cref="MontoFondoRevolvente"/>.
    /// </summary>
    /// <param name="valor">Importe en MXN. Debe ser positivo y no exceder el límite normativo.</param>
    /// <exception cref="MontoExcedidoException">Cuando el valor supera $75,000.00 MXN.</exception>
    /// <exception cref="DomainException">Cuando el valor es cero o negativo.</exception>
    public static MontoFondoRevolvente Crear(decimal valor)
    {
        if (valor <= 0)
            throw new DomainException(
                $"El monto debe ser mayor a cero. Valor recibido: {valor:C2}.");

        if (valor > LimitesNegocio.MontoMaximoFondoRevolvente)
            throw new MontoExcedidoException(valor, LimitesNegocio.MontoMaximoFondoRevolvente);

        return new MontoFondoRevolvente(Math.Round(valor, 2, MidpointRounding.AwayFromZero));
    }

    /// <summary>
    /// Intenta crear una instancia sin lanzar excepción.
    /// Útil para validaciones previas en el ViewModel antes de enviar al dominio.
    /// </summary>
    public static bool TryCrear(decimal valor, out MontoFondoRevolvente? monto, out string? error)
    {
        monto = null;
        error = null;

        if (valor <= 0)
        {
            error = "El monto debe ser mayor a cero.";
            return false;
        }

        if (valor > LimitesNegocio.MontoMaximoFondoRevolvente)
        {
            error = $"El monto ${valor:N2} MXN excede el límite máximo de " +
                    $"${LimitesNegocio.MontoMaximoFondoRevolvente:N2} MXN (RN-001).";
            return false;
        }

        monto = new MontoFondoRevolvente(Math.Round(valor, 2, MidpointRounding.AwayFromZero));
        return true;
    }

    // -------------------------------------------------------------------------
    // Propiedades calculadas — apoyo a la capa de presentación (MontoDisplay)
    // -------------------------------------------------------------------------

    /// <summary>Porcentaje consumido del límite normativo. Rango [0.0, 1.0+].</summary>
    public double PorcentajeDelLimite =>
        (double)(Valor / LimitesNegocio.MontoMaximoFondoRevolvente);

    /// <summary>
    /// Clasifica el nivel de alerta para el componente MontoDisplay de la UI.
    /// Verde (&lt;70%), Amarillo (70–90%), Rojo (&gt;90%).
    /// </summary>
    public NivelAlertaMonto NivelAlerta => PorcentajeDelLimite switch
    {
        < 0.70 => NivelAlertaMonto.Verde,
        < 0.90 => NivelAlertaMonto.Amarillo,
        _      => NivelAlertaMonto.Rojo
    };

    /// <summary>Indica si este monto es el resultado de un cálculo con IVA incluido.</summary>
    public MontoFondoRevolvente AgregarIva(decimal tasaIva = 0.16m)
    {
        var conIva = Valor * (1 + tasaIva);
        return Crear(conIva); // propaga RN-001 si el total con IVA excede el límite
    }

    /// <summary>Suma dos montos. Valida que el resultado no exceda el límite normativo.</summary>
    public static MontoFondoRevolvente operator +(MontoFondoRevolvente a, MontoFondoRevolvente b)
        => Crear(a.Valor + b.Valor);

    // -------------------------------------------------------------------------
    // Presentación
    // -------------------------------------------------------------------------

    /// <summary>Formato normalizado es-MX: "$XX,XXX.XX MXN"</summary>
    public override string ToString() =>
        Valor.ToString("C2", new System.Globalization.CultureInfo("es-MX")) + " MXN";
}

/// <summary>Nivel de alerta visual para el componente MontoDisplay (§4.3 del README).</summary>
public enum NivelAlertaMonto
{
    /// <summary>Menos del 70% del límite. Barra verde.</summary>
    Verde,
    /// <summary>Entre 70% y 90% del límite. Barra amarilla.</summary>
    Amarillo,
    /// <summary>Más del 90% del límite o excedido. Barra roja.</summary>
    Rojo
}

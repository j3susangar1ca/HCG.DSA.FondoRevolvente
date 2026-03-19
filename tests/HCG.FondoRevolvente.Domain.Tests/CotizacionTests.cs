using HCG.FondoRevolvente.Domain.Aggregates;
using HCG.FondoRevolvente.Domain.Exceptions;
using HCG.FondoRevolvente.Domain.ValueObjects;
using Xunit;

namespace HCG.FondoRevolvente.Domain.Tests;

public class CotizacionTests
{
    private static MontoFondoRevolvente TestMonto => MontoFondoRevolvente.Crear(1000);

    [Fact]
    public void Constructor_AsignaPropiedadesCorrectamente()
    {
        // Arrange
        var solicitudId = 1;
        var proveedorId = 2;
        var numero = "COT-001";
        var subtotal = TestMonto;
        var tasaIva = 0.16m;
        var emision = DateTime.UtcNow.AddDays(-1);
        var vigencia = DateTime.UtcNow.AddDays(30);

        // Act
        var cot = new Cotizacion(solicitudId, proveedorId, numero, subtotal, tasaIva, emision, vigencia, "user1");

        // Assert
        Assert.Equal(solicitudId, cot.SolicitudId);
        Assert.Equal(proveedorId, cot.ProveedorId);
        Assert.Equal(numero, cot.NumeroCotizacion);
        Assert.Equal(subtotal, cot.MontoSubtotal);
        Assert.Equal(160, cot.MontoIva);
        Assert.Equal(1160, cot.MontoTotal.Valor);
        Assert.False(cot.Seleccionada);
        Assert.True(cot.EstaVigente);
    }

    [Fact]
    public void Actualizar_ModificaPropiedades()
    {
        // Arrange
        var cot = new Cotizacion(1, 1, "001", TestMonto, 0.16m, DateTime.UtcNow, DateTime.UtcNow.AddDays(10));

        // Act
        cot.Actualizar("002", MontoFondoRevolvente.Crear(2000), 0.08m, DateTime.UtcNow, DateTime.UtcNow.AddDays(20), "Entrega inmediata", 5, "Nota");

        // Assert
        Assert.Equal("002", cot.NumeroCotizacion);
        Assert.Equal(2000, cot.MontoSubtotal.Valor);
        Assert.Equal(0.08m, cot.TasaIva);
        Assert.Equal(160, cot.MontoIva);
        Assert.Equal(2160, cot.MontoTotal.Valor);
        Assert.Equal(5, cot.DiasEntrega);
    }

    [Fact]
    public void SeleccionarComoGanadora_CambiaEstado()
    {
        // Arrange
        var cot = new Cotizacion(1, 1, "001", TestMonto, 0.16m, DateTime.UtcNow, DateTime.UtcNow.AddDays(10));

        // Act
        cot.SeleccionarComoGanadora("Monto más bajo");

        // Assert
        Assert.True(cot.Seleccionada);
        Assert.Equal("Monto más bajo", cot.RazonSeleccion);
        Assert.NotNull(cot.FechaSeleccion);
    }

    [Fact]
    public void DiferenciaPorcentual_CalculaCorrectamente()
    {
        // Arrange
        var cot1 = new Cotizacion(1, 1, "001", MontoFondoRevolvente.Crear(100), 0, DateTime.UtcNow, DateTime.UtcNow.AddDays(10));
        var cot2 = new Cotizacion(1, 2, "002", MontoFondoRevolvente.Crear(150), 0, DateTime.UtcNow, DateTime.UtcNow.AddDays(10));

        // Act
        var dif = cot2.DiferenciaPorcentualRespectoA(cot1);

        // Assert
        Assert.Equal(50, dif); // (150-100)/100 * 100 = 50%
    }
}

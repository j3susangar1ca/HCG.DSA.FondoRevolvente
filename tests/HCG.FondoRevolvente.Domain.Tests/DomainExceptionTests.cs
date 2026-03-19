using HCG.FondoRevolvente.Domain.Exceptions;
using Xunit;

namespace HCG.FondoRevolvente.Domain.Tests;

public class DomainExceptionTests
{
    [Fact]
    public void DomainException_ConCodigoYMensaje_AsignaCorrectamente()
    {
        // Act
        var ex = new DomainException("ERROR_TEST", "Mensaje de prueba");

        // Assert
        Assert.Equal("ERROR_TEST", ex.CodigoError);
        Assert.Equal("Mensaje de prueba", ex.Message);
    }

    [Fact]
    public void ConDatos_AgregaInformacionContextual()
    {
        // Arrange
        var ex = new DomainException("ERROR_TEST", "Prueba");

        // Act
        ex.ConDatos("IdSolicitud", 123)
          .ConDatos("Usuario", "admin");

        // Assert
        Assert.Equal(123, ex.DatosAdicionales["IdSolicitud"]);
        Assert.Equal("admin", ex.DatosAdicionales["Usuario"]);
    }

    [Fact]
    public void MontoExcedidoException_TieneCodigoCorrecto()
    {
        // Act
        var ex = new MontoExcedidoException(80_000m, 75_000m);

        // Assert
        Assert.Equal("MONTO_EXCEDIDO_RN001", ex.CodigoError);
        Assert.Contains("80,000", ex.Message);
    }

    [Fact]
    public void BloqueoEdicionException_TieneCodigoCorrecto()
    {
        // Act
        var ex = new BloqueoEdicionException("DSA-2026-001", "juan.perez", DateTime.Now);

        // Assert
        Assert.Equal("BLOQUEO_EDICION_RN005", ex.CodigoError);
    }
}

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
        Assert.Equal("RN001_MONTO_EXCEDIDO", ex.CodigoError);
        Assert.Contains("80,000", ex.Message);
    }

    [Fact]
    public void BloqueoEdicionException_TieneCodigoCorrecto()
    {
        // Act
        var ex = new BloqueoEdicionException(123, "Solicitud", "juan.perez", DateTime.Now.AddMinutes(-10), DateTime.Now);

        // Assert
        Assert.Equal("RN005_BLOQUEO_EDICION", ex.CodigoError);
    }

    [Fact]
    public void FraccionamientoDetectadoException_TieneCodigoYMascarraCorrectos()
    {
        // Act
        var ex = new FraccionamientoDetectadoException(
            "ABCD123456XXX", 
            150_000m, 
            2, 
            90, 
            new[] { "FOL-1", "FOL-2" });

        // Assert
        Assert.Equal("RN002_FRACCIONAMIENTO_DETECTADO", ex.CodigoError);
        Assert.Equal("ABCD123456XXX", ex.RfcProveedor); // Raw property
        Assert.Equal("ABCD****XXX", ex.DatosAdicionales["RfcProveedor"]); // Masked data
        Assert.Contains("150,000", ex.Message);
    }

    [Fact]
    public void CotizacionesInsuficientesException_SoportaMonto()
    {
        // Act
        var ex = new CotizacionesInsuficientesException(2, 80_000m);

        // Assert
        Assert.Equal("RN003_COTIZACIONES_INSUFICIENTES", ex.CodigoError);
        Assert.Equal(3, ex.CotizacionesRequeridas);
        Assert.Equal(1, ex.CotizacionesFaltantes);
    }

    [Fact]
    public void SatServicioNoDisponibleException_CalculaTiempoReintento()
    {
        // Act
        var ex = new SatServicioNoDisponibleException(3);

        // Assert
        Assert.Equal("RN004_SAT_NO_DISPONIBLE", ex.CodigoError);
        Assert.Equal(TimeSpan.FromMinutes(5), ex.TiempoReintentoSugerido); // Basado en LimitesNegocio.MinutosCircuitoAbierto
    }

    [Fact]
    public void TransicionInvalidaException_GuardaEstados()
    {
        // Act
        var ex = new TransicionInvalidaException(
            Enums.EstadoSolicitud.Pagado, 
            Enums.EstadoSolicitud.Recepcionado, 
            "No se puede retroceder");

        // Assert
        Assert.Equal("TRANSICION_INVALIDA", ex.CodigoError);
        Assert.Equal(Enums.EstadoSolicitud.Pagado, ex.EstadoActual);
        Assert.Equal(Enums.EstadoSolicitud.Recepcionado, ex.EstadoDestino);
    }
}

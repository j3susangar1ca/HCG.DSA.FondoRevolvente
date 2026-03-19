using HCG.FondoRevolvente.Domain.Constants;
using Xunit;

namespace HCG.FondoRevolvente.Domain.Tests;

public class LimitesNegocioTests
{
    [Fact]
    public void CotizacionesRequeridas_SiempreRetornaTres()
    {
        // Arrange
        decimal montoBajo = 500m;
        decimal montoAlto = 100_000m;

        // Act
        var resultBajo = CotizacionesRequeridas.ObtenerCotizacionesRequeridas(montoBajo);
        var resultAlto = CotizacionesRequeridas.ObtenerCotizacionesRequeridas(montoAlto);

        // Assert
        Assert.Equal(3, resultBajo);
        Assert.Equal(3, resultAlto);
    }

    [Fact]
    public void CotizacionesRequeridas_EsSuficiente_FuncionaCorrectamente()
    {
        // Act & Assert
        Assert.True(CotizacionesRequeridas.EsSuficiente(1000m, 3));
        Assert.True(CotizacionesRequeridas.EsSuficiente(1000m, 4));
        Assert.False(CotizacionesRequeridas.EsSuficiente(1000m, 2));
    }

    [Fact]
    public void ConfiguracionBloqueo_EstaExpirado_DetectaExpiracion()
    {
        // Arrange
        var ahora = DateTime.UtcNow;
        var adquisicionExpirada = ahora.AddMinutes(-(LimitesNegocio.MinutosDuracionBloqueo + 1));
        var adquisicionReciente = ahora.AddMinutes(-(LimitesNegocio.MinutosDuracionBloqueo - 1));

        // Act
        var resultExpirado = ConfiguracionBloqueo.EstaExpirado(adquisicionExpirada, ahora);
        var resultReciente = ConfiguracionBloqueo.EstaExpirado(adquisicionReciente, ahora);

        // Assert
        Assert.True(resultExpirado);
        Assert.False(resultReciente);
    }

    [Fact]
    public void ConfiguracionBloqueo_DebeRenovar_DetectaVentanaRenovacion()
    {
        // Arrange
        var ahora = DateTime.UtcNow;
        // Ventana de renovación: faltan <= 5 min para expirar (30 min total)
        // O sea, han pasado entre 25 y 30 minutos.
        var adquisicionParaRenovar = ahora.AddMinutes(-26); 
        var adquisicionMuyReciente = ahora.AddMinutes(-10);

        // Act
        var resultRenovar = ConfiguracionBloqueo.DebeRenovar(adquisicionParaRenovar, ahora);
        var resultNoRenovar = ConfiguracionBloqueo.DebeRenovar(adquisicionMuyReciente, ahora);

        // Assert
        Assert.True(resultRenovar);
        Assert.False(resultNoRenovar);
    }
}

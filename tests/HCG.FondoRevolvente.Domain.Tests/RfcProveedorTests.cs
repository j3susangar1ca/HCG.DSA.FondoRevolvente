using HCG.FondoRevolvente.Domain.Aggregates;
using HCG.FondoRevolvente.Domain.Exceptions;
using HCG.FondoRevolvente.Domain.ValueObjects;
using Xunit;

namespace HCG.FondoRevolvente.Domain.Tests;

public class RfcProveedorTests
{
    [Fact]
    public void Crear_PersonaMoral_Valida()
    {
        // Arrange
        var rfcStr = "ABC120520XYZ";

        // Act
        var rfc = RfcProveedor.Crear(rfcStr);

        // Assert
        Assert.Equal("ABC120520XYZ", rfc.Valor);
        Assert.True(rfc.EsPersonaMoral);
        Assert.False(rfc.EsPersonaFisica);
        Assert.Equal("ABC", rfc.Siglas);
        Assert.Equal("XYZ", rfc.Homoclave);
    }

    [Fact]
    public void Crear_PersonaFisica_Valida()
    {
        // Arrange
        var rfcStr = "ABCD800101XYZ";

        // Act
        var rfc = RfcProveedor.Crear(rfcStr);

        // Assert
        Assert.Equal("ABCD800101XYZ", rfc.Valor);
        Assert.True(rfc.EsPersonaFisica);
        Assert.False(rfc.EsPersonaMoral);
        Assert.Equal("ABCD", rfc.Siglas);
    }

    [Fact]
    public void Crear_RfcInvalido_LanzaExcepcion()
    {
        // Act & Assert
        Assert.Throws<DomainException>(() => RfcProveedor.Crear("INVALIDO"));
        Assert.Throws<DomainException>(() => RfcProveedor.Crear("ABC120520XY")); // Corto
        Assert.Throws<DomainException>(() => RfcProveedor.Crear("ABC120520XYZW")); // Largo
    }

    [Fact]
    public void FechaDeclarada_ExtraeCorrectamente()
    {
        // Arrange
        var rfcMoral = RfcProveedor.Crear("ABC120520XYZ");
        var rfcFisica = RfcProveedor.Crear("ABCD800101XYZ");

        // Act
        var fechaMoral = rfcMoral.FechaDeclarada;
        var fechaFisica = rfcFisica.FechaDeclarada;

        // Assert
        Assert.Equal(new DateOnly(2012, 5, 20), fechaMoral);
        Assert.Equal(new DateOnly(1980, 1, 1), fechaFisica);
    }

    [Fact]
    public void Enmascarado_MuestraFormatoCorrecto()
    {
        // Arrange
        var rfc = RfcProveedor.Crear("ABCD123456XYZ");

        // Act
        var enmascarado = rfc.Enmascarado();

        // Assert
        Assert.Equal("ABCD****XYZ", enmascarado);
    }

    [Fact]
    public void TryCrear_RetornaResultadoEsperado()
    {
        // Act
        var resultadoOk = RfcProveedor.TryCrear("ABC120520XYZ", out var rfc, out var error);
        var resultadoFail = RfcProveedor.TryCrear("INVALIDO", out var rfcFail, out var errorFail);

        // Assert
        Assert.True(resultadoOk);
        Assert.NotNull(rfc);
        Assert.Null(error);

        Assert.False(resultadoFail);
        Assert.Null(rfcFail);
        Assert.NotNull(errorFail);
    }
}

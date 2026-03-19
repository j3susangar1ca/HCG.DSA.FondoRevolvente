using HCG.FondoRevolvente.Domain.Aggregates;
using HCG.FondoRevolvente.Domain.Exceptions;
using HCG.FondoRevolvente.Domain.ValueObjects;
using Xunit;

namespace HCG.FondoRevolvente.Domain.Tests;

public class ProveedorTests
{
    private static RfcProveedor TestRfc => RfcProveedor.Crear("ABC120520XYZ");

    [Fact]
    public void Constructor_AsignaPropiedadesCorrectamente()
    {
        // Arrange
        var rfc = TestRfc;
        var razonSocial = "Proveedor de Pruebas";
        var email = "contacto@proveedor.com";
        var usuario = "test.user";

        // Act
        var proveedor = new Proveedor(rfc, razonSocial, email, usuario);

        // Assert
        Assert.Equal(rfc, proveedor.Rfc);
        Assert.Equal(razonSocial, proveedor.RazonSocial);
        Assert.Equal(email, proveedor.Email);
        Assert.Equal(usuario, proveedor.UsuarioRegistro);
        Assert.True(proveedor.Activo);
        Assert.NotEqual(default, proveedor.FechaRegistro);
    }

    [Fact]
    public void ActualizarDatos_ModificaPropiedades()
    {
        // Arrange
        var proveedor = new Proveedor(TestRfc, "Original", "old@test.com");

        // Act
        proveedor.ActualizarDatos(
            "Nuevo Nombre",
            "Nombre Comercial",
            "new@test.com",
            "3312345678",
            "Av. Independencia 123",
            "44100",
            "Notas de prueba");

        // Assert
        Assert.Equal("Nuevo Nombre", proveedor.RazonSocial);
        Assert.Equal("Nombre Comercial", proveedor.NombreComercial);
        Assert.Equal("new@test.com", proveedor.Email);
        Assert.Equal("3312345678", proveedor.Telefono);
        Assert.NotNull(proveedor.FechaUltimaModificacion);
    }

    [Fact]
    public void Desactivar_CambiaEstadoAnexaNota()
    {
        // Arrange
        var proveedor = new Proveedor(TestRfc, "Test", "test@test.com");

        // Act
        proveedor.Desactivar("Incumplimiento de contrato");

        // Assert
        Assert.False(proveedor.Activo);
        Assert.Contains("Incumplimiento", proveedor.Notas);
    }

    [Fact]
    public void RegistrarValidacionSat_ActualizaEstado()
    {
        // Arrange
        var proveedor = new Proveedor(TestRfc, "Test", "test@test.com");

        // Act
        proveedor.RegistrarValidacionSat(true);

        // Assert
        Assert.True(proveedor.RfcValidadoSat);
        Assert.NotNull(proveedor.FechaUltimaValidacionSat);
    }
}

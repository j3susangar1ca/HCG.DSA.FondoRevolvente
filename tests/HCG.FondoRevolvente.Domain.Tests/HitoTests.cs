using HCG.FondoRevolvente.Domain.Aggregates;
using HCG.FondoRevolvente.Domain.Enums;
using HCG.FondoRevolvente.Domain.ValueObjects;
using Xunit;

namespace HCG.FondoRevolvente.Domain.Tests;

public class HitoTests
{
    [Fact]
    public void Constructor_AsignaPropiedadesCorrectamente()
    {
        // Arrange
        var solicitudId = 1;
        var tipo = TipoHito.RecepcionOficio;
        var estado = EstadoSolicitud.Recepcionado;
        var usuario = "test.user";

        // Act
        var hito = new Hito(solicitudId, tipo, estado, usuario);

        // Assert
        Assert.Equal(solicitudId, hito.SolicitudId);
        Assert.Equal(tipo, hito.Tipo);
        Assert.Equal(estado, hito.EstadoEnElMomento);
        Assert.Equal(usuario, hito.Usuario);
        Assert.NotEqual(default, hito.FechaHora);
        Assert.Equal(FaseProceso.RecepcionValidacionInicial, hito.Fase);
    }

    [Fact]
    public void ConDatosAdicionales_SerializaCorrectamente()
    {
        // Arrange
        var hito = new Hito(1, TipoHito.RecepcionOficio, EstadoSolicitud.Recepcionado);
        var datos = new Dictionary<string, object> { ["Key"] = "Value", ["Num"] = 123 };

        // Act
        hito.ConDatosAdicionales(datos);
        var recuperados = hito.ObtenerDatosAdicionales();

        // Assert
        Assert.NotNull(hito.DatosAdicionales);
        Assert.Equal("Value", recuperados?["Key"].ToString());
    }

    [Fact]
    public void FactoryMethods_CreaHitosConDatosCorrectos()
    {
        // Act
        var hitoSolicitud = Hito.SolicitudCreada(1, "user", "Nombre", RolAplicacion.CompradorDSA);
        var hitoPago = Hito.PagoRealizado(1, 500.50m, "finanzas");

        // Assert
        Assert.Equal(TipoHito.RecepcionOficio, hitoSolicitud.Tipo);
        Assert.Equal(EstadoSolicitud.Recepcionado, hitoSolicitud.EstadoEnElMomento);
        
        Assert.Equal(TipoHito.ConfirmacionPagoEjecutado, hitoPago.Tipo);
        Assert.Equal(EstadoSolicitud.Pagado, hitoPago.EstadoEnElMomento);
        Assert.Contains("500.5", hitoPago.DatosAdicionales!);
    }
}

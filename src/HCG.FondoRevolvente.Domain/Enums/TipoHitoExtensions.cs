namespace HCG.FondoRevolvente.Domain.Enums;

/// <summary>
/// Proporciona utilidades para manejar los tipos de hitos en relación con las fases.
/// </summary>
public static class TipoHitoExtensions
{
    /// <summary>
    /// Obtiene los tipos de hitos aplicables o esperados en una fase específica.
    /// Esta lógica mapea los eventos significativos del ciclo de vida.
    /// </summary>
    public static IEnumerable<TipoHito> ObtenerHitosPorFase(FaseProceso fase)
    {
        return fase switch
        {
            FaseProceso.RecepcionValidacionInicial => new[] 
            { 
                TipoHito.Creacion, 
                TipoHito.CambioEstado, 
                TipoHito.Observacion 
            },
            FaseProceso.AutorizacionCAA => new[] 
            { 
                TipoHito.CambioEstado, 
                TipoHito.Observacion, 
                TipoHito.DocumentoAdjunto 
            },
            FaseProceso.EstudioMercadoCotizacion => new[] 
            { 
                TipoHito.DocumentoAdjunto, // Cotización
                TipoHito.CambioEstado, 
                TipoHito.Observacion 
            },
            FaseProceso.SeleccionProveedorPedido => new[] 
            { 
                TipoHito.CambioEstado, 
                TipoHito.DocumentoAdjunto, // Orden de compra
                TipoHito.Observacion 
            },
            FaseProceso.EntregaBienesServicios => new[] 
            { 
                TipoHito.DocumentoAdjunto, // Acta de entrega
                TipoHito.CambioEstado, 
                TipoHito.Observacion 
            },
            FaseProceso.ValidacionFiscalCFDI => new[] 
            { 
                TipoHito.ValidacionFiscal, 
                TipoHito.DocumentoAdjunto, // CFDI XML/PDF
                TipoHito.CambioEstado 
            },
            FaseProceso.Pago => new[] 
            { 
                TipoHito.CambioEstado, 
                TipoHito.Observacion, 
                TipoHito.DocumentoAdjunto // Comprobante de transferencia
            },
            FaseProceso.CierreExpediente => new[] 
            { 
                TipoHito.CambioEstado, 
                TipoHito.Observacion 
            },
            _ => Array.Empty<TipoHito>()
        };
    }
}

namespace HCG.FondoRevolvente.Domain.Enums;

/// <summary>
/// Define las fases macro del proceso de adquisición por Fondo Revolvente.
/// Utilizado por el componente Stepper/PhaseIndicator en la UI (§Módulo 06).
/// </summary>
public enum FaseProceso
{
    /// <summary>Fase 1: Recepción y Validación Inicial.</summary>
    RecepcionValidacionInicial = 1,

    /// <summary>Fase 2: Autorización por el Comité de Adquisiciones y Arrendamientos.</summary>
    AutorizacionCAA = 2,

    /// <summary>Fase 3: Estudio de Mercado / Cotización.</summary>
    EstudioMercadoCotizacion = 3,

    /// <summary>Fase 4: Selección de Proveedor / Pedido.</summary>
    SeleccionProveedorPedido = 4,

    /// <summary>Fase 5: Entrega de Bienes o Servicios.</summary>
    EntregaBienesServicios = 5,

    /// <summary>Fase 6: Validación Fiscal CFDI.</summary>
    ValidacionFiscalCFDI = 6,

    /// <summary>Fase 7: Pago.</summary>
    Pago = 7,

    /// <summary>Fase 8: Cierre del Expediente.</summary>
    CierreExpediente = 8
}

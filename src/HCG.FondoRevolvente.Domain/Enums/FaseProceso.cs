namespace HCG.FondoRevolvente.Domain.Enums;

/// <summary>
/// Define las fases macro del proceso de adquisición por Fondo Revolvente.
/// Utilizado por el componente Stepper/PhaseIndicator en la UI (§Módulo 06).
/// </summary>
public enum FaseProceso
{
    /// <summary>Fase 1: Preparación y captura de la solicitud.</summary>
    Captura,

    /// <summary>Fase 2: Cotizaciones y selección de proveedor ganador (RN-003).</summary>
    Cotizacion,

    /// <summary>Fase 3: Validación fiscal del CFDI ante el SAT (RN-004).</summary>
    ValidacionFiscal,

    /// <summary>Fase 4: Revisión técnica y autorización administrativa.</summary>
    Autorizacion,

    /// <summary>Fase 5: Proceso de pago y cierre del expediente.</summary>
    Pago
}

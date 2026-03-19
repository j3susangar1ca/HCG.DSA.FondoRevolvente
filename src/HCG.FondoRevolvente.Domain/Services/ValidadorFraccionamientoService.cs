using HCG.FondoRevolvente.Domain.Aggregates;
using HCG.FondoRevolvente.Domain.Exceptions;
using HCG.FondoRevolvente.Domain.Interfaces;
using HCG.FondoRevolvente.Domain.Constants;

namespace HCG.FondoRevolvente.Domain.Services;

/// <summary>
/// Valida la RM-002: Impedir el fraccionamiento de compras.
/// Verifica que un código de producto no haya sido adquirido por el mismo
/// solicitante o área en el presente ejercicio fiscal.
/// </summary>
public class ValidadorFraccionamientoService
{
    private readonly IRepository<Solicitud, int> _solicitudRepository;

    public ValidadorFraccionamientoService(IRepository<Solicitud, int> solicitudRepository)
    {
        _solicitudRepository = solicitudRepository;
    }

    /// <summary>
    /// Ejecuta la validación de fraccionamiento.
    /// </summary>
    /// <param name="solicitud">La solicitud actual.</param>
    /// <param name="codigoProducto">El código del producto/servicio a validar.</param>
    /// <exception cref="FraccionamientoDetectadoException">Si se detecta compra previa en el mismo ejercicio.</exception>
    public async Task ValidarAsync(Solicitud solicitud, string codigoProducto, CancellationToken ct = default)
    {
        var todas = await _solicitudRepository.GetAllAsync(ct);

        var previa = todas.FirstOrDefault(s =>
            s.Id != solicitud.Id &&
            s.Folio.EjercicioFiscal == solicitud.Folio.EjercicioFiscal &&
            s.Descripcion.Contains(codigoProducto, StringComparison.OrdinalIgnoreCase));

        if (previa != null)
        {
            throw new FraccionamientoDetectadoException(
                rfcProveedor: "PENDIENTE", 
                sumaOperaciones: 0, 
                numeroOperaciones: 1, 
                diasPeriodo: LimitesNegocio.MesesDeteccionFraccionamiento * 30,
                foliosRelacionados: new[] { previa.Folio.Valor });
        }
    }
}

using MediatR;
using HCG.FondoRevolvente.Application.Common.Models;
using HCG.FondoRevolvente.Application.Interfaces;
using HCG.FondoRevolvente.Domain.Aggregates;
using HCG.FondoRevolvente.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

using HCG.FondoRevolvente.Domain.Services;
using HCG.FondoRevolvente.Domain.Constants;

namespace HCG.FondoRevolvente.Application.Solicitudes.Commands.CrearSolicitud;

public class CrearSolicitudCommandHandler : IRequestHandler<CrearSolicitudCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeService _dateTimeService;
    private readonly ValidadorFraccionamientoService _validadorFraccionamiento;

    public CrearSolicitudCommandHandler(
        IApplicationDbContext context, 
        ICurrentUserService currentUserService,
        IDateTimeService dateTimeService,
        ValidadorFraccionamientoService validadorFraccionamiento)
    {
        _context = context;
        _currentUserService = currentUserService;
        _dateTimeService = dateTimeService;
        _validadorFraccionamiento = validadorFraccionamiento;
    }

    public async Task<Result<int>> Handle(CrearSolicitudCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var monto = MontoFondoRevolvente.Crear(request.Monto);

            // RN-002: Detección de Fraccionamiento
            if (request.ProveedorId.HasValue)
            {
                var proveedor = await _context.Proveedores
                    .FirstOrDefaultAsync(p => p.Id == request.ProveedorId, cancellationToken);

                if (proveedor != null)
                {
                    var fechaLimite = _dateTimeService.Now.AddMonths(-LimitesNegocio.MesesDeteccionFraccionamiento);

                    var solicitudesPrevias = await _context.Solicitudes
                        .Where(s => s.FechaCreacion >= fechaLimite)
                        .SelectMany(s => s.Cotizaciones)
                        .Where(c => c.Seleccionada && c.ProveedorId == request.ProveedorId)
                        .Select(c => new SolicitudPreviaProveedor
                        {
                            Folio = c.Solicitud!.Folio.Valor,
                            Monto = c.MontoTotal.Valor,
                            Fecha = c.Solicitud.FechaCreacion,
                            Estado = c.Solicitud.Estado.ToString()
                        })
                        .ToListAsync(cancellationToken);

                    _validadorFraccionamiento.ValidarOViolacion(monto.Valor, proveedor.Rfc, solicitudesPrevias);
                }
            }
            
            // Generar folio (Lógica simple para demo, idealmente un DomainService con repo)
            var anio = _dateTimeService.Now.Year;
            var ultimoSecuencial = await _context.Solicitudes
                .Where(s => s.Folio.EjercicioFiscal == anio)
                .CountAsync(cancellationToken);
            
            var folio = FolioDSA.Generar(anio, ultimoSecuencial + 1);

            var entidad = new Solicitud(
                folio, 
                monto, 
                request.Descripcion, 
                _currentUserService.UserName ?? "Sistema");

            _context.Solicitudes.Add(entidad);

            await _context.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(entidad.Id);
        }
        catch (Exception ex)
        {
            return Result<int>.Failure(ex.Message);
        }
    }
}

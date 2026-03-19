using MediatR;
using HCG.FondoRevolvente.Application.Common.Models;
using HCG.FondoRevolvente.Application.Interfaces;
using HCG.FondoRevolvente.Domain.Aggregates;
using HCG.FondoRevolvente.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace HCG.FondoRevolvente.Application.Solicitudes.Commands.CrearSolicitud;

public class CrearSolicitudCommandHandler : IRequestHandler<CrearSolicitudCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeService _dateTimeService;

    public CrearSolicitudCommandHandler(
        IApplicationDbContext context, 
        ICurrentUserService currentUserService,
        IDateTimeService dateTimeService)
    {
        _context = context;
        _currentUserService = currentUserService;
        _dateTimeService = dateTimeService;
    }

    public async Task<Result<int>> Handle(CrearSolicitudCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var monto = MontoFondoRevolvente.Crear(request.Monto);
            
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

using FluentValidation;

namespace HCG.FondoRevolvente.Application.Solicitudes.Commands.CrearSolicitud;

public class CrearSolicitudCommandValidator : AbstractValidator<CrearSolicitudCommand>
{
    public CrearSolicitudCommandValidator()
    {
        RuleFor(v => v.Descripcion)
            .NotEmpty().WithMessage("La descripción es obligatoria.")
            .MaximumLength(500).WithMessage("La descripción no puede exceder los 500 caracteres.");

        RuleFor(v => v.Monto)
            .GreaterThan(0).WithMessage("El monto debe ser mayor a cero.");
    }
}

using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Improvement.Commands.Update
{
    public class UpdateImprovementCommandValidator : AbstractValidator<UpdateImprovementCommand>
    {
        public UpdateImprovementCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El Id de la mejora debe ser mayor que 0.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre de la mejora es obligatorio.")
                .MaximumLength(150).WithMessage("El nombre no debe exceder los 150 caracteres.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripción de la mejora es obligatoria.")
                .MaximumLength(250).WithMessage("La descripción no debe exceder los 250 caracteres.");
        }
    }
}

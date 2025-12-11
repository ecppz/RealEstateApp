using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Improvement.Commands.Delete
{
    public class DeleteImprovementCommandValidator : AbstractValidator<DeleteImprovementCommand>
    {
        public DeleteImprovementCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El Id de la mejora debe ser mayor que 0.");
        }
    }
}

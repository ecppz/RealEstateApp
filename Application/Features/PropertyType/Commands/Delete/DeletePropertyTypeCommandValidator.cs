using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.PropertyType.Commands.Delete
{
    public class DeletePropertyTypeCommandValidator : AbstractValidator<DeletePropertyTypeCommand>
    {
        public DeletePropertyTypeCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }

}

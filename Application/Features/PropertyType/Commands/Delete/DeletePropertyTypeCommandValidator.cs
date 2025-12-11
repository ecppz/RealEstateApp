using FluentValidation;

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

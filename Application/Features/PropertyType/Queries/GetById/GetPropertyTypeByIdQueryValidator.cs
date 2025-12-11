using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.PropertyType.Queries.GetById
{
    public class GetPropertyTypeByIdQueryValidator : AbstractValidator<GetPropertyTypeByIdQuery>
    {
        public GetPropertyTypeByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }

}

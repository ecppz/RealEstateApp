using Domain.Interfaces;
using MediatR;
using Domain.Entities;

namespace Application.Features.PropertyType.Commands.Create
{
    public class CreatePropertyTypeCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CreatePropertyTypeCommandHandler : IRequestHandler<CreatePropertyTypeCommand, int>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;

        public CreatePropertyTypeCommandHandler(IPropertyTypeRepository propertyTypeRepository)
        {
            _propertyTypeRepository = propertyTypeRepository;
        }

        public async Task<int> Handle(CreatePropertyTypeCommand command, CancellationToken cancellationToken)
        {
            Domain.Entities.PropertyType entity = new()
            {
                Id = 0,
                Name = command.Name,
                Description = command.Description
            };

            var result = await _propertyTypeRepository.AddPropertyAsync(entity);

            if (result == null)
            {
                throw new Exception("Error creating property type");
            }

            return result.Id;
        }
    }

}

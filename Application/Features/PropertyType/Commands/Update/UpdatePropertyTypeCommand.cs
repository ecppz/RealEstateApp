using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.PropertyType.Commands.Update
{
    public class UpdatePropertyTypeCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UpdatePropertyTypeCommandHandler : IRequestHandler<UpdatePropertyTypeCommand, int>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;

        public UpdatePropertyTypeCommandHandler(IPropertyTypeRepository propertyTypeRepository)
        {
            _propertyTypeRepository = propertyTypeRepository;
        }

        public async Task<int> Handle(UpdatePropertyTypeCommand command, CancellationToken cancellationToken)
        {
            Domain.Entities.PropertyType entity = new()
            {
                Id = 0,
                Name = command.Name,
                Description = command.Description
            };

            var result = await _propertyTypeRepository.UpdatePropertyAsync(command.Id, entity);

            if (result == null)
            {
                throw new Exception("Error updating property type");
            }

            return result.Id;
        }
    }

}

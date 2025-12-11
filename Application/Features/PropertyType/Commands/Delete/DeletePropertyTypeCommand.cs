using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.PropertyType.Commands.Delete
{
    public class DeletePropertyTypeCommand : IRequest<int?>
    {
        public int Id { get; set; }
    }

    public class DeletePropertyTypeCommandHandler : IRequestHandler<DeletePropertyTypeCommand, int?>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;

        public DeletePropertyTypeCommandHandler(IPropertyTypeRepository propertyTypeRepository)
        {
            _propertyTypeRepository = propertyTypeRepository;
        }

        public async Task<int?> Handle(DeletePropertyTypeCommand command, CancellationToken cancellationToken)
        {
            var propertyType = await _propertyTypeRepository.GetPropertyById(command.Id);

            if (propertyType == null)
            {
                return null; 
            }

            await _propertyTypeRepository.DeletePropertyAsync(command.Id);
            return command.Id;
        }
    }

}

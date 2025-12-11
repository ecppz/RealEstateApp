using Domain.Interfaces;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace Application.Features.PropertyType.Commands.Update
{

    /// <summary>
    /// Parameters required to update an existing property type record.
    /// </summary>
    public class UpdatePropertyTypeCommand
    {
        /// <example>Apartamento</example>
        [SwaggerParameter(Description = "The new name of the property type")]
        public string Name { get; set; }

        /// <example>Apartamento remodelado</example>
        [SwaggerParameter(Description = "The new description of the property type")]
        public string Description { get; set; }
    }

    /// <summary>
    /// Wrapper command that includes the Id of the property type to update.
    /// </summary>
    public class UpdatePropertyTypeCommandWrapper : IRequest<int>
    {
        /// <example>5</example>
        [SwaggerParameter(Description = "The unique identifier of the property type to update")]
        public int Id { get; set; }

        /// <example>Apartamento</example>
        [SwaggerParameter(Description = "The new name of the property type")]
        public string Name { get; set; }

        /// <example>Apartamento remodelado</example>
        [SwaggerParameter(Description = "The new description of the property type")]
        public string Description { get; set; }
    }

    /// <summary>
    /// Handles the update of an existing property type record.
    /// </summary>
    public class UpdatePropertyTypeCommandHandler : IRequestHandler<UpdatePropertyTypeCommandWrapper, int>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;

        public UpdatePropertyTypeCommandHandler(IPropertyTypeRepository propertyTypeRepository)
        {
            _propertyTypeRepository = propertyTypeRepository;
        }

        /// <summary>
        /// Executes the update of the property type by Id.
        /// </summary>
        /// <param name="command">The update command containing Id, Name, and Description.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>
        /// The Id of the updated property type if successful; otherwise throws an exception.
        /// </returns>
        public async Task<int> Handle(UpdatePropertyTypeCommandWrapper command, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.PropertyType
            {
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

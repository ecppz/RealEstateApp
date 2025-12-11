using Domain.Interfaces;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace Application.Features.PropertyType.Commands.Delete
{
    /// <summary>
    /// Parameters required to delete an existing property type record.
    /// </summary>
    public class DeletePropertyTypeCommand : IRequest<int?>
    {
        /// <example>5</example>
        [SwaggerParameter(Description = "The unique identifier of the property type to delete")]
        public int Id { get; set; }
    }

    /// <summary>
    /// Handles the deletion of a property type record.
    /// </summary>
    public class DeletePropertyTypeCommandHandler : IRequestHandler<DeletePropertyTypeCommand, int?>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;

        public DeletePropertyTypeCommandHandler(IPropertyTypeRepository propertyTypeRepository)
        {
            _propertyTypeRepository = propertyTypeRepository;
        }

        /// <summary>
        /// Executes the deletion of the property type by Id.
        /// </summary>
        /// <param name="command">The delete command containing the Id of the property type.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>
        /// The Id of the deleted property type if successful; otherwise <c>null</c> if not found.
        /// </returns>
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

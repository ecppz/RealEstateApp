using Domain.Interfaces;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SaleType.Commands.Update
{


    /// <summary>
    /// Parameters required to update an existing sale type record.
    /// </summary>
    public class UpdateSaleTypeCommand : IRequest<int>
    {
        /// <example>3</example>
        [SwaggerParameter(Description = "The unique identifier of the sale type to update")]
        public int Id { get; set; }

        /// <example>Permuta</example>
        [SwaggerParameter(Description = "The new name of the sale type")]
        public string Name { get; set; }

        /// <example>Intercambio de propiedades</example>
        [SwaggerParameter(Description = "The new description of the sale type")]
        public string Description { get; set; }
    }

    /// <summary>
    /// Handles the update of an existing sale type record.
    /// </summary>
    public class UpdateSaleTypeCommandHandler : IRequestHandler<UpdateSaleTypeCommand, int>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;

        public UpdateSaleTypeCommandHandler(ISaleTypeRepository saleTypeRepository)
        {
            _saleTypeRepository = saleTypeRepository;
        }

        /// <summary>
        /// Executes the update of a sale type by Id.
        /// </summary>
        /// <param name="command">The update command containing Id, Name, and Description.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>The Id of the updated sale type.</returns>
        public async Task<int> Handle(UpdateSaleTypeCommand command, CancellationToken cancellationToken)
        {
            Domain.Entities.SaleType entity = new()
            {
                Name = command.Name,
                Description = command.Description
            };

            var result = await _saleTypeRepository.UpdateAsync(command.Id, entity);

            if (result == null)
            {
                throw new Exception("Error updating sale type");
            }

            return result.Id;
        }
    }



}

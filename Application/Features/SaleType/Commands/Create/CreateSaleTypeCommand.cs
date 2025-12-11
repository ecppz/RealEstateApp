using Domain.Interfaces;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SaleType.Commands.Create
{

    /// <summary>
    /// Parameters required to create a new sale type record.
    /// </summary>
    public class CreateSaleTypeCommand : IRequest<int>
    {
        /// <example>Venta Directa</example>
        [SwaggerParameter(Description = "The name of the sale type")]
        public string Name { get; set; }

        /// <example>Venta sin intermediarios</example>
        [SwaggerParameter(Description = "The description of the sale type")]
        public string Description { get; set; }
    }

    /// <summary>
    /// Handles the creation of a new sale type record.
    /// </summary>
    public class CreateSaleTypeCommandHandler : IRequestHandler<CreateSaleTypeCommand, int>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;

        public CreateSaleTypeCommandHandler(ISaleTypeRepository saleTypeRepository)
        {
            _saleTypeRepository = saleTypeRepository;
        }

        /// <summary>
        /// Executes the creation of a sale type.
        /// </summary>
        /// <param name="command">The create command containing Name and Description.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>The Id of the created sale type.</returns>
        public async Task<int> Handle(CreateSaleTypeCommand command, CancellationToken cancellationToken)
        {
            Domain.Entities.SaleType entity = new()
            {
                Name = command.Name,
                Description = command.Description
            };

            var result = await _saleTypeRepository.AddAsync(entity);

            if (result == null)
            {
                throw new Exception("Error creating sale type");
            }

            return result.Id;
        }
    }

}

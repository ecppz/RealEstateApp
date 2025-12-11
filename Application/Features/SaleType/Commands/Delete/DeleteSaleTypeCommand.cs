using Domain.Interfaces;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SaleType.Commands.Delete
{
    /// <summary>
    /// Parameters required to delete an existing sale type record.
    /// </summary>
    public class DeleteSaleTypeCommand : IRequest<int?>
    {
        /// <example>3</example>
        [SwaggerParameter(Description = "The unique identifier of the sale type to delete")]
        public int Id { get; set; }
    }

    /// <summary>
    /// Handles the deletion of a sale type record.
    /// </summary>
    public class DeleteSaleTypeCommandHandler : IRequestHandler<DeleteSaleTypeCommand, int?>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;

        public DeleteSaleTypeCommandHandler(ISaleTypeRepository saleTypeRepository)
        {
            _saleTypeRepository = saleTypeRepository;
        }

        /// <summary>
        /// Executes the deletion of a sale type by Id.
        /// </summary>
        /// <param name="command">The delete command containing the Id of the sale type.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>
        /// The Id of the deleted sale type if successful; otherwise <c>null</c> if not found.
        /// </returns>
        public async Task<int?> Handle(DeleteSaleTypeCommand command, CancellationToken cancellationToken)
        {
            var saleType = await _saleTypeRepository.GetById(command.Id);

            if (saleType == null)
            {
                return null;
            }

            await _saleTypeRepository.DeleteAsync(command.Id);
            return command.Id;
        }
    }

}

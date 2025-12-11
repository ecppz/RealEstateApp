using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SaleType.Commands.Delete
{
    public class DeleteSaleTypeCommand : IRequest<int?>
    {
        public int Id { get; set; }
    }

    public class DeleteSaleTypeCommandHandler : IRequestHandler<DeleteSaleTypeCommand, int?>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;

        public DeleteSaleTypeCommandHandler(ISaleTypeRepository saleTypeRepository)
        {
            _saleTypeRepository = saleTypeRepository;
        }

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

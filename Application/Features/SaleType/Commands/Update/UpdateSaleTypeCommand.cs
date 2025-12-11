using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SaleType.Commands.Update
{
    public class UpdateSaleTypeCommand : IRequest<int>
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
    }


    public class UpdateSaleTypeCommandHandler : IRequestHandler<UpdateSaleTypeCommand, int>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;

        public UpdateSaleTypeCommandHandler(ISaleTypeRepository saleTypeRepository)
        {
            _saleTypeRepository = saleTypeRepository;
        }

        public async Task<int> Handle(UpdateSaleTypeCommand command, CancellationToken cancellationToken)
        {
           Domain.Entities.SaleType entity = new ()
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

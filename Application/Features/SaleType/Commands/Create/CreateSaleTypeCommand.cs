using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SaleType.Commands.Create
{

    public class CreateSaleTypeCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CreateSaleTypeCommandHandler : IRequestHandler<CreateSaleTypeCommand, int>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;

        public CreateSaleTypeCommandHandler(ISaleTypeRepository saleTypeRepository)
        {
            _saleTypeRepository = saleTypeRepository;
        }

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

using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Improvement.Commands.Create
{
    public class CreateImprovementCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CreateImprovementCommandHandler : IRequestHandler<CreateImprovementCommand, int>
    {
        private readonly IImprovementRepository _improvementRepository;

        public CreateImprovementCommandHandler(IImprovementRepository improvementRepository)
        {
            _improvementRepository = improvementRepository;
        }

        public async Task<int> Handle(CreateImprovementCommand command, CancellationToken cancellationToken)
        {
            Domain.Entities.Improvement entity = new() 
            {
                Name = command.Name,
                Description = command.Description
            };

            var result = await _improvementRepository.AddAsync(entity);

            if (result == null)
            {
                throw new Exception("Error creating improvement");
            }

            return result.Id;
        }
    }


}

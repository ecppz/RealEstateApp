using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Improvement.Commands.Update
{
    public class UpdateImprovementCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UpdateImprovementCommandHandler : IRequestHandler<UpdateImprovementCommand, int>
    {
        private readonly IImprovementRepository _improvementRepository;

        public UpdateImprovementCommandHandler(IImprovementRepository improvementRepository)
        {
            _improvementRepository = improvementRepository;
        }

        public async Task<int> Handle(UpdateImprovementCommand command, CancellationToken cancellationToken)
        {
           Domain.Entities.Improvement  entity = new()
            {
                Name = command.Name,
                Description = command.Description
            };

            var result = await _improvementRepository.UpdateAsync(command.Id, entity);

            if (result == null)
            {
                throw new Exception("Error updating improvement");
            }

            return result.Id;
        }
    }




}

using Domain.Interfaces;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Improvement.Commands.Create
{
    /// <summary>
    /// Parameters required to create a new improvement record.
    /// </summary>
    public class CreateImprovementCommand : IRequest<int>
    {
        /// <example>Piscina</example>
        [SwaggerParameter(Description = "The name of the improvement")]
        public string Name { get; set; }

        /// <example>Construcción de piscina en el patio</example>
        [SwaggerParameter(Description = "The description of the improvement")]
        public string Description { get; set; }
    }

    /// <summary>
    /// Handles the creation of a new improvement record.
    /// </summary>
    public class CreateImprovementCommandHandler : IRequestHandler<CreateImprovementCommand, int>
    {
        private readonly IImprovementRepository _improvementRepository;

        public CreateImprovementCommandHandler(IImprovementRepository improvementRepository)
        {
            _improvementRepository = improvementRepository;
        }

        /// <summary>
        /// Executes the creation of an improvement.
        /// </summary>
        /// <param name="command">The create command containing Name and Description.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>The Id of the created improvement.</returns>
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

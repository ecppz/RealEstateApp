using Domain.Interfaces;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Improvement.Commands.Update
{
    /// <summary>
    /// Parameters required to update an existing improvement record.
    /// </summary>
    public class UpdateImprovementCommand : IRequest<int>
    {
        /// <example>7</example>
        [SwaggerParameter(Description = "The unique identifier of the improvement to update")]
        public int Id { get; set; }

        /// <example>Terraza</example>
        [SwaggerParameter(Description = "The new name of the improvement")]
        public string Name { get; set; }

        /// <example>Espacio abierto con pérgola</example>
        [SwaggerParameter(Description = "The new description of the improvement")]
        public string Description { get; set; }
    }

    /// <summary>
    /// Handles the update of an existing improvement record.
    /// </summary>
    public class UpdateImprovementCommandHandler : IRequestHandler<UpdateImprovementCommand, int>
    {
        private readonly IImprovementRepository _improvementRepository;

        public UpdateImprovementCommandHandler(IImprovementRepository improvementRepository)
        {
            _improvementRepository = improvementRepository;
        }

        /// <summary>
        /// Executes the update of an improvement by Id.
        /// </summary>
        /// <param name="command">The update command containing Id, Name, and Description.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>The Id of the updated improvement.</returns>
        public async Task<int> Handle(UpdateImprovementCommand command, CancellationToken cancellationToken)
        {
            Domain.Entities.Improvement entity = new()
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

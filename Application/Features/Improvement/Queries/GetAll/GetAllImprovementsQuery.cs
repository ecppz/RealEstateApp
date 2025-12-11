using Application.Dtos.Improvement;
using Application.Dtos.Property;
using Application.Dtos.PropertyImprovement;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Improvement.Queries.GetAll
{
    /// <summary>
    /// Query to retrieve all improvements with their basic details.
    /// </summary>
    public class GetAllImprovementsQuery : IRequest<List<ImprovementSimpleDto>>
    {
        // No parameters required for this query
    }

    /// <summary>
    /// Handles the retrieval of all improvements.
    /// </summary>
    public class GetAllImprovementsQueryHandler : IRequestHandler<GetAllImprovementsQuery, List<ImprovementSimpleDto>>
    {
        private readonly IImprovementRepository _improvementRepository;

        public GetAllImprovementsQueryHandler(IImprovementRepository improvementRepository)
        {
            _improvementRepository = improvementRepository;
        }

        /// <summary>
        /// Executes the query to get all improvements.
        /// </summary>
        /// <param name="query">The query request (no parameters required).</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>
        /// A list of <see cref="ImprovementSimpleDto"/> objects containing Id, Name, and Description.
        /// Returns an empty list if no improvements are found (translated to 204 NoContent in the controller).
        /// </returns>
        public async Task<List<ImprovementSimpleDto>> Handle(GetAllImprovementsQuery query, CancellationToken cancellationToken)
        {
            var improvements = await _improvementRepository.GetAllList();

            if (improvements == null || !improvements.Any())
            {
                return new List<ImprovementSimpleDto>();
            }

            return improvements.Select(improvement => new ImprovementSimpleDto
            {
                Id = improvement.Id,
                Name = improvement.Name,
                Description = improvement.Description
            }).ToList();
        }
    }

}

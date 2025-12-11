using Application.Dtos.Improvement;
using Application.Dtos.PropertyImprovement;
using Domain.Interfaces;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Improvement.Queries.GetById
{
    /// <summary>
    /// Query to retrieve an improvement by its unique identifier.
    /// </summary>
    public class GetImprovementByIdQuery : IRequest<ImprovementSimpleDto?>
    {
        /// <example>7</example>
        [SwaggerParameter(Description = "The unique identifier of the improvement to retrieve")]
        public int Id { get; set; }
    }

    /// <summary>
    /// Handles the retrieval of an improvement by Id.
    /// </summary>
    public class GetImprovementByIdQueryHandler : IRequestHandler<GetImprovementByIdQuery, ImprovementSimpleDto?>
    {
        private readonly IImprovementRepository _improvementRepository;

        public GetImprovementByIdQueryHandler(IImprovementRepository improvementRepository)
        {
            _improvementRepository = improvementRepository;
        }

        /// <summary>
        /// Executes the query to get an improvement by Id.
        /// </summary>
        /// <param name="query">The query containing the Id of the improvement.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>
        /// A <see cref="ImprovementSimpleDto"/> with Id, Name, and Description if found;
        /// otherwise <c>null</c> (translated to 204 NoContent in the controller).
        /// </returns>
        public async Task<ImprovementSimpleDto?> Handle(GetImprovementByIdQuery query, CancellationToken cancellationToken)
        {
            var improvement = await _improvementRepository.GetById(query.Id);

            if (improvement == null)
            {
                return null;
            }

            return new ImprovementSimpleDto
            {
                Id = improvement.Id,
                Name = improvement.Name,
                Description = improvement.Description
            };
        }
    }

}

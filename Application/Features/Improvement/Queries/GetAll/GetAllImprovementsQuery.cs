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
    public class GetAllImprovementsQuery : IRequest<List<ImprovementSimpleDto>>
    {
    }

    public class GetAllImprovementsQueryHandler : IRequestHandler<GetAllImprovementsQuery, List<ImprovementSimpleDto>>
    {
        private readonly IImprovementRepository _improvementRepository;

        public GetAllImprovementsQueryHandler(IImprovementRepository improvementRepository)
        {
            _improvementRepository = improvementRepository;
        }

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

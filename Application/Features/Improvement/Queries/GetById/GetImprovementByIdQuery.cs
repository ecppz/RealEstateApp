using Application.Dtos.Improvement;
using Application.Dtos.PropertyImprovement;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Improvement.Queries.GetById
{
    public class GetImprovementByIdQuery : IRequest<ImprovementSimpleDto?>
    {
        public int Id { get; set; }
    }


    public class GetImprovementByIdQueryHandler : IRequestHandler<GetImprovementByIdQuery, ImprovementSimpleDto?>
    {
        private readonly IImprovementRepository _improvementRepository;

        public GetImprovementByIdQueryHandler(IImprovementRepository improvementRepository)
        {
            _improvementRepository = improvementRepository;
        }

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

using Application.Dtos.Agent;
using Application.Interfaces;
using Domain.Common.Enums;
using MediatR;

namespace Application.Features.Agents.Queries.GetAll
{
    public class GetAllAgentsQuery : IRequest<IList<AgentDto>>
    {
        public bool OnlyActive { get; set; } = true; 
    }

    public class GetAllAgentsQueryHandler : IRequestHandler<GetAllAgentsQuery, IList<AgentDto>>
    {
        private readonly IBaseAccountService accountService;

        public GetAllAgentsQueryHandler(IBaseAccountService accountService)
        {
            this.accountService = accountService;
        }

        public async Task<IList<AgentDto>> Handle(GetAllAgentsQuery query, CancellationToken cancellationToken)
        {
            var agents = await accountService.GetAllUsers<AgentDto>(
                isActive: query.OnlyActive,
                role: Roles.Agent
            );

            return agents;
        }
    }
}

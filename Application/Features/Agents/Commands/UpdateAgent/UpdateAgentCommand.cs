using Application.Dtos.Agent;
using Application.Dtos.User;
using Application.Interfaces;
using AutoMapper;
using Domain.Common.Enums;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace Application.Features.Agents.Commands.UpdateAgent
{
    /// <summary>
    /// Command to update only the status of an agent
    /// </summary>
    public class UpdateAgentStatusCommand : IRequest<bool>
    {
        /// <example>5</example>
        [SwaggerParameter(Description = "The unique identifier of the agent to update")]
        public string? AgentId { get; set; }

        /// <example>true</example>
        [SwaggerParameter(Description = "The new status of the agent (true=active, false=inactive)")]
        public bool IsActive { get; set; }
    }

    public class UpdateAgentStatusCommandHandler : IRequestHandler<UpdateAgentStatusCommand, bool>
    {
        private readonly IUserAccountServiceForWebApi accountServiceForWebApi;
        private readonly IMapper mapper;

        public UpdateAgentStatusCommandHandler(IUserAccountServiceForWebApi accountServiceForWebApi, IMapper mapper)
        {
            this.accountServiceForWebApi = accountServiceForWebApi;
            this.mapper = mapper;
        }
        public async Task<bool> Handle(UpdateAgentStatusCommand command, CancellationToken cancellationToken)
        {
            var agent = await accountServiceForWebApi.GetUserById<AgentDto>(command.AgentId);

            if (agent == null)
                return false;

            agent.Status = command.IsActive ? UserStatus.Active : UserStatus.Inactive;

            var saveDto = mapper.Map<SaveUserDto>(agent);

            var result = await accountServiceForWebApi.EditUser(saveDto, "Admin", null, false, true);

            return true;
        }

    }
    }

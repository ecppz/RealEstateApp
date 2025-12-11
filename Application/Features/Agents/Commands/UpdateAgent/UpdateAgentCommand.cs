using Application.Dtos.Agent;
using Application.Dtos.User;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Common.Enums;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Application.Features.Agents.Commands.UpdateAgent
{
    /// <summary>
    /// Command to update only the status of an agent
    /// </summary>
    public class UpdateAgentStatusCommand : IRequest<Unit>
    {
        /// <example>5</example>
        [SwaggerParameter(Description = "The unique identifier of the agent to update")]
        public string? AgentId { get; set; }

        /// <example>true</example>
        [SwaggerParameter(Description = "The new status of the agent (true=active, false=inactive)")]
        public bool IsActive { get; set; }
    }

    public class UpdateAgentStatusCommandHandler : IRequestHandler<UpdateAgentStatusCommand, Unit>
    {
        private readonly IUserAccountServiceForWebApi accountServiceForWebApi;
        private readonly IMapper mapper;

        public UpdateAgentStatusCommandHandler(IUserAccountServiceForWebApi accountServiceForWebApi, IMapper mapper)
        {
            this.accountServiceForWebApi = accountServiceForWebApi;
            this.mapper = mapper;
        }
        public async Task<Unit> Handle(UpdateAgentStatusCommand command, CancellationToken cancellationToken)
        {
        //    var agent = await accountServiceForWebApi.GetUserById<AgentDto>(command.AgentId);

      //      if (agent == null || agent.Role != Roles.Agent)
                throw new ApiException("Agent not found with this id or not an Agent", (int)HttpStatusCode.NotFound);

        //    agent.Status = command.IsActive ? UserStatus.Active : UserStatus.Inactive;

          //  var saveDto = mapper.Map<SaveUserDto>(agent);

            //var result = await accountServiceForWebApi.EditUser(saveDto, "Admin", null, false, true);

            //if (result.HasError)
              //  throw new ApiException(result.Errors.FirstOrDefault() ?? "Error al actualizar estado del agente", (int)HttpStatusCode.InternalServerError);

            //return Unit.Value;
        }



    }
    }

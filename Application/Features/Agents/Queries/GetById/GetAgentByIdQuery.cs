using Application.Dtos.Agent;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Properties.Queries.GetByCode
{
    public class GetAgentByIdQuery : IRequest<AgentDto>
    {
        public required string Id { get; set; }
    }
    public class GetAgentByIdQueryHandler : IRequestHandler<GetAgentByIdQuery, AgentDto>
    {
        private readonly IUserAccountServiceForWebApi userAccountServiceForWebApi;
        private readonly IMapper mapper;

        public GetAgentByIdQueryHandler(IUserAccountServiceForWebApi userAccountServiceForWebApi, IMapper mapper)
        {
            this.userAccountServiceForWebApi = userAccountServiceForWebApi;
            this.mapper = mapper;
        }

        public async Task<AgentDto> Handle(GetAgentByIdQuery query, CancellationToken cancellationToken)
        {
       //     var entity = await userAccountServiceForWebApi.GetUserById<AgentDto>(query.Id);

          //  if (entity == null)
            {
                throw new KeyNotFoundException($"Este agente con este id {query.Id} no se encuentra");
            }

            //return entity;
        }

    }
}

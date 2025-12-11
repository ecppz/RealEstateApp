using Application.Dtos.Agent;
using Application.Dtos.Property;
using Application.Features.Agents.Commands.UpdateAgent;
using Application.Features.Agents.Queries.GetAll;
using Application.Features.Properties.Queries.GetAll;
using Application.Features.Properties.Queries.GetByCode;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstateApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin, Developer")]
    [SwaggerTag("Provides updates and queries for managing agents")]
    public class AgentController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AgentDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Retrieve all agents",
            Description = "Returns a list of all agents with their account details"
        )]
        public async Task<IActionResult> GetAgents([FromQuery] bool onlyActive)
        {
            var agents = await Mediator.Send(new GetAllAgentsQuery { OnlyActive = onlyActive });

            if (agents == null || agents.Count == 0)
                return NoContent();

            return Ok(agents);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgentDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Retrieve agent by ID",
            Description = "Returns the details of a specific agents with their account details"
        )]
        public async Task<IActionResult> GetById(string id)
        {
            var agentId = await Mediator.Send(new GetAgentByIdQuery { Id = id });

            if (agentId == null)
            {
                return NotFound();
            }

            return Ok(agentId);
        }

        [HttpGet("agents/{agentId}/properties")]
        [Authorize(Roles = "Admin, Developer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PropertyDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [SwaggerOperation(
            Summary = "Retrieve properties by agent",
            Description = "Returns a list of properties assigned to a specific agent, including type, sale details, images, and improvements"
        )]

        public async Task<IActionResult> GetAgentProperties(string agentId, [FromQuery] bool onlyAvailable = false)
        {
            var properties = await Mediator.Send(new GetAgentPropertiesQuery { AgentId = agentId, OnlyAvailable = onlyAvailable });

            if (properties == null || properties.Count == 0)
            {
                return NoContent();
            }

            return Ok(properties);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Update agent status",
            Description = "Allows only Admins to activate or deactivate an agent"
        )]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] bool isActive)
        {
            await Mediator.Send(new UpdateAgentStatusCommand
            {
                AgentId = id,
                IsActive = isActive
            });

            return NoContent(); 
        }



    }
}

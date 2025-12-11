using Application.Dtos.Improvement;
using Application.Features.Improvement.Commands.Create;
using Application.Features.Improvement.Commands.Delete;
using Application.Features.Improvement.Commands.Update;
using Application.Features.Improvement.Queries.GetAll;
using Application.Features.Improvement.Queries.GetById;
using Asp.Versioning;
using Domain.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstateApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ImprovementController : BaseApiController
    {
        /// <summary>
        /// Creates a new improvement.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = nameof(Roles.Admin))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Create a new improvement",
            Description = "Creates a new improvement with the provided data"
        )]
        public async Task<IActionResult> Create([FromBody] ImprovementCreateDto dto)
        {
            var command = new CreateImprovementCommand
            {
                Name = dto.Name,
                Description = dto.Description
            };

            var createdId = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = createdId }, createdId);
        }

        /// <summary>
        /// Updates an existing improvement.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = nameof(Roles.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Update an existing improvement",
            Description = "Updates the specified improvement with the new provided data"
        )]
        public async Task<IActionResult> Update(int id, [FromBody] ImprovementUpdateDto dto)
        {
            var command = new UpdateImprovementCommand
            {
                Id = id,
                Name = dto.Name,
                Description = dto.Description
            };

            var updatedId = await Mediator.Send(command);
            return Ok(updatedId);
        }

        /// <summary>
        /// Gets all improvements.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = $"{nameof(Roles.Admin)},{nameof(Roles.Developer)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Get all improvements",
            Description = "Returns a list of all improvements"
        )]
        public async Task<IActionResult> GetAll()
        {
            var improvements = await Mediator.Send(new GetAllImprovementsQuery());
            if (improvements == null || !improvements.Any())
            {
                return NoContent();
            }
            return Ok(improvements);
        }

        /// <summary>
        /// Gets an improvement by id.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = $"{nameof(Roles.Admin)},{nameof(Roles.Developer)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Get improvement by id",
            Description = "Returns the improvement with the specified id"
        )]
        public async Task<IActionResult> GetById(int id)
        {
            var improvement = await Mediator.Send(new GetImprovementByIdQuery { Id = id });
            if (improvement == null)
            {
                return NoContent();
            }
            return Ok(improvement);
        }

        /// <summary>
        /// Deletes an improvement.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(Roles.Admin))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
         Summary = "Delete improvement",
         Description = "Deletes the improvement with the specified id"
     )]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedId = await Mediator.Send(new DeleteImprovementCommand { Id = id });

            if (deletedId == null)
            {
                return NoContent(); 
            }

            return NoContent(); 
        }
    }
}

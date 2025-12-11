using Application.Dtos.PropertyType;
using Application.Features.PropertyType.Commands.Create;
using Application.Features.PropertyType.Commands.Delete;
using Application.Features.PropertyType.Commands.Update;
using Application.Features.PropertyType.Queries.GetAll;
using Application.Features.PropertyType.Queries.GetById;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstateApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Provides CRUD operations and queries for managing property types")]
    public class PropertyTypeController : BaseApiController
    {
        [HttpGet]
        [Authorize(Roles = "Admin,Developer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PropertyTypeListDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Retrieve all property types",
            Description = "Returns a list of all property types with their details"
        )]
        public async Task<IActionResult> Get()
        {
            var propertyTypes = await Mediator.Send(new GetAllPropertyTypesQuery());

            if (propertyTypes == null || propertyTypes.Count == 0)
            {
                return NoContent();
            }

            return Ok(propertyTypes);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Developer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyTypeListDto))] 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Retrieve property type by ID",
            Description = "Returns the details of a specific property type using its unique identifier"
        )]
        public async Task<IActionResult> Get(int id)
        {
            var propertyType = await Mediator.Send(new GetPropertyTypeByIdQuery { Id = id });

            if (propertyType == null)
            {
                return NoContent();
            }

            return Ok(propertyType);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Create a new property type",
            Description = "Creates a new property type using the provided data"
        )]
        public async Task<IActionResult> Create([FromBody] CreatePropertyTypeCommand command)
        {
            var id = await Mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, id);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Update an existing property type",
            Description = "Updates the specified property type with the new provided data"
        )]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePropertyTypeCommand command)
        {
            var updatedId = await Mediator.Send(new UpdatePropertyTypeCommandWrapper
            {
                Id = id,
                Name = command.Name,
                Description = command.Description
            });

            if (updatedId <= 0)
            {
                return NotFound();
            }

            return Ok(updatedId);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Delete a property type",
            Description = "Deletes the property type associated with the specified ID"
        )]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedId = await Mediator.Send(new DeletePropertyTypeCommand { Id = id });

            if (deletedId <= 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

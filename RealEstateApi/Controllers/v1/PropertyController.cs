using Application.Dtos.Property;
using Application.Features.Properties.Queries.GetAll;
using Application.Features.Properties.Queries.GetByCode;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace RealEstateApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin, Developer")]
    [SwaggerTag("Provides queries for managing properties")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PropertyController : BaseApiController 
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PropertyDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Retrieve all properties",
            Description = "Returns a list of all properties"
        )]
        public async Task<IActionResult> GetProperties([FromQuery] bool onlyAvailable = false)
        {
            var properties = await Mediator.Send(new GetAllPropertiesQuery { OnlyAvailable = onlyAvailable });

            if (properties == null || properties.Count == 0)
                return NoContent();

            return Ok(properties);
        }

        [HttpGet("by-id/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Retrieve property by ID",
            Description = "Returns the details of a specific property using its unique identifier"
        )]
        public async Task<IActionResult> GetById(int id)
        {
            var property = await Mediator.Send(new GetPropertyByIdQuery { Id = id });


            if (property == null)
            {
                return NotFound(new { message = $"Property con id {id} no se encuentra" });
            }

            return Ok(property);
        }

        [HttpGet("by-code/{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Retrieve property by code",
            Description = "Returns the details of a specific property using its code"
)]
        public async Task<IActionResult> GetByCode(string code)
        {
            var property = await Mediator.Send(new GetPropertyByCodeQuery { Code = code });

            if (property == null)
            {
                return NotFound(new { message = $"Property con código {code} no se encuentra" });
            }

            return Ok(property);
        }
    }
}
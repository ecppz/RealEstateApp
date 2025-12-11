using Application.Dtos.SaleType;
using Application.Features.SaleType.Commands.Create;
using Application.Features.SaleType.Commands.Delete;
using Application.Features.SaleType.Commands.Update;
using Application.Features.SaleType.Queries.GetAll;
using Application.Features.SaleType.Queries.GetById;
using Asp.Versioning;
using Domain.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstateApi.Controllers.v1
{
    namespace RealEstateApi.Controllers
    {
        [ApiVersion("1.0")]
        public class SaleTypeController : BaseApiController
        {
            /// <summary>
            /// Creates a new sale type.
            /// </summary>
            [HttpPost]
            [Authorize(Roles = nameof(Roles.Admin))]
            [ProducesResponseType(StatusCodes.Status201Created)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            [SwaggerOperation(
                Summary = "Create a new sale type",
                Description = "Creates a new sale type with the provided data"
            )]
            public async Task<IActionResult> Create([FromBody] CreateSaleTypeCommand command)
            {
                var createdId = await Mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id = createdId }, createdId);
            }

            /// <summary>
            /// Updates an existing sale type.
            /// </summary>
            [HttpPut("{id}")]
            [Authorize(Roles = nameof(Roles.Admin))]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            [SwaggerOperation(
            Summary = "Update an existing sale type",
            Description = "Updates the specified sale type with the new provided data"
        )]
            public async Task<IActionResult> Update(int id, [FromBody] SaleTypeUpdateDto dto)
            {
                var command = new UpdateSaleTypeCommand
                {
                    Id = id,                
                    Name = dto.Name,
                    Description = dto.Description
                };

                var updatedId = await Mediator.Send(command);
                return Ok(updatedId);
            }

            /// <summary>
            /// Gets all sale types.
            /// </summary>
            [HttpGet]
            [Authorize(Roles = $"{nameof(Roles.Admin)},{nameof(Roles.Developer)}")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            [SwaggerOperation(
                Summary = "Get all sale types",
                Description = "Returns a list of all sale types"
            )]
            public async Task<IActionResult> GetAll()
            {
                var saleTypes = await Mediator.Send(new GetAllSaleTypesQuery());
                if (saleTypes == null || !saleTypes.Any())
                {
                    return NoContent();
                }
                return Ok(saleTypes);
            }

            /// <summary>
            /// Gets a sale type by id.
            /// </summary>
            [HttpGet("{id}")]
            [Authorize(Roles = $"{nameof(Roles.Admin)},{nameof(Roles.Developer)}")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            [SwaggerOperation(
                Summary = "Get sale type by id",
                Description = "Returns the sale type with the specified id"
            )]
            public async Task<IActionResult> GetById(int id)
            {
                var saleType = await Mediator.Send(new GetSaleTypeByIdQuery { Id = id });
                if (saleType == null)
                {
                    return NoContent();
                }
                return Ok(saleType);
            }

            /// <summary>
            /// Deletes a sale type.
            /// </summary>
            [HttpDelete("{id}")]
            [Authorize(Roles = nameof(Roles.Admin))]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            [SwaggerOperation(
                 Summary = "Delete sale type",
                 Description = "Deletes the sale type with the specified id"
             )]
            public async Task<IActionResult> Delete(int id)
            {
                var deletedId = await Mediator.Send(new DeleteSaleTypeCommand { Id = id });

                if (deletedId == null)
                {
                    return NoContent(); 
                }

                return NoContent(); 
            }
        }
    }
}

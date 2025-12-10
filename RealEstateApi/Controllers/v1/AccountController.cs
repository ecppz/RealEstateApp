using Application.Dtos.Admin;
using Application.Dtos.Developer;
using Application.Dtos.User;
using Application.Interfaces;
using Asp.Versioning;
using AutoMapper;
using Domain.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApi.Controllers;
using Swashbuckle.AspNetCore.Annotations;

namespace InvestmentApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Endpoints for user authentication, registration, and account recovery")]
    public class AccountController(IUserAccountServiceForWebApi userAccountServiceForWebApi) : BaseApiController
    {
        private readonly IUserAccountServiceForWebApi _userAccountServiceForWebApi = userAccountServiceForWebApi;

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Authenticate user",
            Description = "Validates user credentials and returns an authentication token with user information"
        )]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(await _userAccountServiceForWebApi.AuthenticateAsync(dto));
        }



        [Authorize(Roles = "Admin")]
        [HttpPost("register-admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Register a new admin",
            Description = "Creates a new admin account (only accessible by logged-in Admins)"
        )]
        public async Task<IActionResult> RegisterAdmin([FromForm] CreateAdminForApiDto dto, [FromServices] IMapper mapper)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var save = mapper.Map<SaveUserDto>(dto);

                var result = await _userAccountServiceForWebApi.RegisterUser(save, null, dto.DocumentNumber ,true);

            if (result == null || result.HasError)
                return BadRequest(result?.Errors);

            return StatusCode(StatusCodes.Status201Created);
        }

        [Authorize(Roles = "Admin, Developer")]
        [HttpPost("register-developer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Register a new developer",
            Description = "Creates a new developer account (accessible by Admins and Developers)"
        )]
        public async Task<IActionResult> RegisterDeveloper([FromForm] CreateDeveloperForApiDto dto, [FromServices] IMapper mapper)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (dto.Role != RolesForApi.Developer)
                return BadRequest("Solo se permite crear Developers");

            var save = mapper.Map<SaveUserDto>(dto);

            var result = await _userAccountServiceForWebApi.RegisterUser(save, null, dto.DocumentNumber, true);

            if (result == null || result.HasError)
                return BadRequest(result?.Errors);


            return StatusCode(StatusCodes.Status201Created);
        }



    }

}

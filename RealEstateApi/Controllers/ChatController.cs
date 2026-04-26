using Application.Dtos.Chat;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstateApi.Controllers
{
    [ApiController]
    [Route("chat")]
    [AllowAnonymous]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChatResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Asistente conversacional de propiedades",
            Description = "Simula un chat IA para recomendar propiedades con base en el sistema de scoring existente.")]
        public ActionResult<ChatResponseDto> Chat([FromBody] ChatRequestDto request)
        {
            try
            {
                var response = _chatService.ProcessMessage(request);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

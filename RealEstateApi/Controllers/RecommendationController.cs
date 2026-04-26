using Application.Dtos.Recommendation;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstateApi.Controllers
{
    [ApiController]
    [Route("recommend")]
    [AllowAnonymous]
    public class RecommendationController : ControllerBase
    {
        private readonly IRecommendationService _recommendationService;

        public RecommendationController(IRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RecommendResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Recomendar propiedad según preferencias",
            Description = "Calcula scores normalizados (precio 40%, habitaciones 20%, tamaño 20%, tipo 20%) y devuelve la mejor opción con explicación en lenguaje natural.")]
        public ActionResult<RecommendResponseDto> Recommend([FromBody] RecommendRequestDto request)
        {
            try
            {
                var result = _recommendationService.Recommend(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

using Domain.Interfaces;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;


namespace Application.Features.Improvement.Commands.Delete
{
    /// <summary>
    /// Parameters required to delete an existing improvement record.
    /// </summary>
    public class DeleteImprovementCommand : IRequest<int?>
    {
        /// <example>7</example>
        [SwaggerParameter(Description = "The unique identifier of the improvement to delete")]
        public int Id { get; set; }
    }

    /// <summary>
    /// Handles the deletion of an improvement record.
    /// </summary>
    public class DeleteImprovementCommandHandler : IRequestHandler<DeleteImprovementCommand, int?>
    {
        private readonly IImprovementRepository _improvementRepository;

        public DeleteImprovementCommandHandler(IImprovementRepository improvementRepository)
        {
            _improvementRepository = improvementRepository;
        }

        /// <summary>
        /// Executes the deletion of an improvement by Id.
        /// </summary>
        /// <param name="command">The delete command containing the Id of the improvement.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>
        /// The Id of the deleted improvement if successful; otherwise <c>null</c> if not found.
        /// </returns>
        public async Task<int?> Handle(DeleteImprovementCommand command, CancellationToken cancellationToken)
        {
            var improvement = await _improvementRepository.GetById(command.Id);

            if (improvement == null)
            {
                return null;
            }

            await _improvementRepository.DeleteAsync(command.Id);
            return command.Id;
        }
    }

}

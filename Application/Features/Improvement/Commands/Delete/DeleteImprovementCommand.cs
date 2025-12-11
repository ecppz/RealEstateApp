using Domain.Interfaces;
using MediatR;


namespace Application.Features.Improvement.Commands.Delete
{
    public class DeleteImprovementCommand : IRequest<int?>
    {
        public int Id { get; set; }
    }

    public class DeleteImprovementCommandHandler : IRequestHandler<DeleteImprovementCommand, int?>
    {
        private readonly IImprovementRepository _improvementRepository;

        public DeleteImprovementCommandHandler(IImprovementRepository improvementRepository)
        {
            _improvementRepository = improvementRepository;
        }

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

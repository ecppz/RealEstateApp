using Application.Dtos.Agent;
using Application.Interfaces;
using FluentValidation;

namespace Application.Features.Agents.Commands.UpdateAgent
{
    public class UpdateAgentStatusCommandValidator : AbstractValidator<UpdateAgentStatusCommand>
    {
        private readonly IUserAccountServiceForWebApi accountService;

        public UpdateAgentStatusCommandValidator(IUserAccountServiceForWebApi accountService)
        {
            this.accountService = accountService;

            RuleFor(a => a.AgentId)
                .NotEmpty().WithMessage("Agent ID is required.")
                .MustAsync(async (id, cancellation) =>
                {
                    var agent = await accountService.GetUserById<AgentDto>(id);
                    return agent != null;
                }).WithMessage("Agent does not exist.");
        }
    }
}

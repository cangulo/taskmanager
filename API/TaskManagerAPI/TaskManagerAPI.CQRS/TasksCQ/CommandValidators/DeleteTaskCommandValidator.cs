using FluentValidation;
using TaskManagerAPI.CQRS.TasksCQ.Commands;

namespace TaskManagerAPI.CQRS.TasksCQ.CommandValidators
{
    public class DeleteTaskCommandValidator : AbstractValidator<DeleteTaskCommand>
    {
        public DeleteTaskCommandValidator()
        {
            RuleFor(m => m.Id).GreaterThan(0);
        }
    }
}

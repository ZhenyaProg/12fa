using FluentValidation;
using TFA.Application.Exceptions;

namespace TFA.Application.UseCases.CreateTopic;

internal class CreateTopicCommandValidator : AbstractValidator<CreateTopicCommand>
{
    public CreateTopicCommandValidator()
    {
        RuleFor(c => c.ForumId)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.Empty);

        RuleFor(c => c.Title)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.Empty)
            .MaximumLength(100)
            .WithErrorCode(ValidationErrorCodes.Toolong);
    }
}
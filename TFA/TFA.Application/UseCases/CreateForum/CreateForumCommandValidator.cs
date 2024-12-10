using FluentValidation;
using TFA.Application.Exceptions;

namespace TFA.Application.UseCases.CreateForum;

internal class CreateForumCommandValidator : AbstractValidator<CreateForumCommand>
{
    public CreateForumCommandValidator()
    {
        RuleFor(cmd => cmd.Title)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.Empty)
            .MaximumLength(50)
            .WithErrorCode(ValidationErrorCodes.Toolong);
    }
}
using FluentValidation;
using TFA.Application.Exceptions;

namespace TFA.Application.UseCases.GetTopics;

internal class GetTopicsQueryValidator : AbstractValidator<GetTopicsQuery>
{
    public GetTopicsQueryValidator()
    {
        RuleFor(q => q.ForumId)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.Empty);

        RuleFor(q => q.Skip)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode(ValidationErrorCodes.Invalid);

        RuleFor(q => q.Take)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode(ValidationErrorCodes.Invalid);
    }
}
﻿using FluentValidation;

namespace TFA.Application.UseCases.CreateTopic;

public class CreateTopicCommandValidator : AbstractValidator<CreateTopicCommand>
{
    public CreateTopicCommandValidator()
    {
        RuleFor(c => c.ForumId)
            .NotEmpty()
            .WithErrorCode("Empty");

        RuleFor(c => c.Title)
            .NotEmpty()
            .WithErrorCode("Empty")
            .MaximumLength(100)
            .WithErrorCode("Toolong");
    }
}
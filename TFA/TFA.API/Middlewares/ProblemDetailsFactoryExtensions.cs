using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TFA.Application.Authorization;
using TFA.Application.Exceptions;

namespace TFA.API.Middlewares;

public static class ProblemDetailsFactoryExtensions
{
    public static ProblemDetails CreateFrom(
        this ProblemDetailsFactory detailsFactory,
        HttpContext httpContext,
        IntentionManagerException exception)
    {
        return detailsFactory.CreateProblemDetails(
            httpContext,
            StatusCodes.Status403Forbidden,
            "Authorization failed",
            detail: exception.Message);
    }

    public static ProblemDetails CreateFrom(
        this ProblemDetailsFactory detailsFactory,
        HttpContext httpContext,
        DomainException exception)
    {
        return detailsFactory.CreateProblemDetails(
            httpContext,
            exception.ErrorCode switch
            {
                ErrorCodes.Gone => StatusCodes.Status410Gone,
                _ => StatusCodes.Status500InternalServerError
            },
            exception.Message);
    }

    public static ProblemDetails CreateFrom(
        this ProblemDetailsFactory detailsFactory,
        HttpContext httpContext,
        ValidationException exception)
    {
        ModelStateDictionary modelStateDictionary = new ModelStateDictionary();

        foreach (var error in exception.Errors)
        {
            modelStateDictionary.AddModelError(error.PropertyName, error.ErrorCode);
        }

        return detailsFactory.CreateValidationProblemDetails(
            httpContext,
            modelStateDictionary,
            StatusCodes.Status400BadRequest,
            "Validation failed");
    }
}
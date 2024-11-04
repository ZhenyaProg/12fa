using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TFA.Application.Authorization;
using TFA.Application.Exceptions;

namespace TFA.API.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext httpContext,
        ProblemDetailsFactory problemDetailsFactory)
    {
        try
        {
            await _next.Invoke(httpContext);
        }
        catch (Exception ex)
        {
            var problemDetails = ex switch
            {
                IntentionManagerException intentionManagerException =>  problemDetailsFactory.CreateFrom(httpContext, intentionManagerException),
                ValidationException validationException => problemDetailsFactory.CreateFrom(httpContext, validationException),
                DomainException domainException => problemDetailsFactory.CreateFrom(httpContext, domainException),
                _ => problemDetailsFactory.CreateProblemDetails(httpContext, StatusCodes.Status500InternalServerError, "Unhandled error!", detail: ex.Message)
            };

            httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            if (problemDetails is ValidationProblemDetails validationProblemDetails)
                await httpContext.Response.WriteAsJsonAsync(validationProblemDetails);
            else
                await httpContext.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
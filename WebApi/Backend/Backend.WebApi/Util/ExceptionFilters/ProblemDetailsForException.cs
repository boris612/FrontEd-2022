using Backend.WebApi.Util.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Backend.WebApi.Util.ExceptionFilters
{
  public class ProblemDetailsForException : ExceptionFilterAttribute
  {
    private readonly ILogger<ProblemDetailsForException> logger;

    public ProblemDetailsForException(ILogger<ProblemDetailsForException> logger)
    {
      this.logger = logger;
    }

    public override void OnException(ExceptionContext context)
    {
      string exceptionMessage = context.Exception.CompleteExceptionMessage();
      logger.LogError("Error 500", exceptionMessage); //TO DO: Log data from context.ActionDescriptor?
      context.ExceptionHandled = true;
      var problemDetails = new ProblemDetails
      {
        Detail = exceptionMessage,
        Title = "Internal server error",
        Instance = context.HttpContext.TraceIdentifier
      };
      context.Result = new ObjectResult(problemDetails)
      {
        ContentTypes = { "application/problem+json" },
        StatusCode = StatusCodes.Status500InternalServerError
      };
    }
  }
}

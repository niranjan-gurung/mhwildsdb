using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace mhwildsdb.Filters
{
    public class ValidateFilter<T>(
        ILogger<ValidateFilter<T>> _logger) : IAsyncActionFilter where T : class
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();

            if (validator == null)
            {
                await next();
                return;
            }

            var model = context.ActionArguments.Values.OfType<T>().FirstOrDefault();

            if (model == null)
            {
                context.Result = new BadRequestObjectResult("Request body is required.");
                return;
            }

            var validationResult = await validator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for {Type}: {Errors}", 
                    typeof(T).Name, validationResult.ToDictionary());

                context.Result = new BadRequestObjectResult(
                    new ValidationProblemDetails(validationResult.ToDictionary())
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Validation Failed"
                    });
                return;
            }

            await next();
        }
    }
}

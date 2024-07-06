using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace API.Extensions;
public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new
                {
                    Field = x.Key,
                    Message = x.Value.Errors.First().ErrorMessage
                }).ToList();

            var errorResponse = new
            {
                Errors = errors
            };

            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = 422
            };
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
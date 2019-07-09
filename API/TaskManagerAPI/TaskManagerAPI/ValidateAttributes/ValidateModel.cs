
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagerAPI.ValidateAttributes
{
    /// <summary>
    /// Thanks to https://www.jerriepelser.com/blog/validation-response-aspnet-core-webapi/
    /// NOT NEEDED BECAUSE MVC ALREADY CHECK IT
    /// </summary>
    public class ValidateModel : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}

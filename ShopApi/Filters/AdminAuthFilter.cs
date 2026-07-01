using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shop.Domain.Models;
using System.Linq;

namespace Shop.App.Filters;

public class AdminAuthFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var user = context.ActionArguments.Values.OfType<User>().FirstOrDefault();

        if (user != null && user.Login == "admin" && user.Id == 1)
        {
            base.OnActionExecuting(context);
        }
        else
        {
            context.Result = new JsonResult(new { message = "No authorization" })
            {
                StatusCode = 401
            };
        }
    }
}

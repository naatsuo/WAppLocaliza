using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WAppLocaliza.Entities;

namespace WAppLocaliza.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeOperatorUserAttribute : Attribute, IAuthorizationFilter
    {
        private string? _role;
        public AuthorizeOperatorUserAttribute()
        {
            _role = null;
        }
        public AuthorizeOperatorUserAttribute(string role)
        {
            _role = role;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.Items["User"] as OperatorUser;
            if (user == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
            else if (!string.IsNullOrEmpty(_role))
            {
                if (user.Roles == null || !user.Roles.Contains(_role))
                {
                    context.Result = new JsonResult(new { message = "Unauthorized" })
                    {
                        StatusCode = StatusCodes.Status401Unauthorized
                    };
                }
            }
        }
    }
}

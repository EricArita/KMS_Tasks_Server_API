using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace MB.Infrastructure.Misc
{
    public class AuthorizationHeader_Param_OperationFilter : IOperationFilter
    {
        // For each request called from Swagger, we apply this filter to create more fields in it
        // (in this case, we want to add an authorization header param in our request for routes
        // that needs to be authorized)
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var isMethodAuthorized = context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
                && !context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();
            var isParentControllerAuthorized = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
                && !context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();

            // If there is no authorize attribute or there is allow anonymous attribute, we skip adding authorization header scheme
            if (isMethodAuthorized || isParentControllerAuthorized)
            {
                // Add possible responses for these routes that needs authorization
                operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

                var jwtbearerScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                };

                operation.Security = new List<OpenApiSecurityRequirement>()
                {
                    new OpenApiSecurityRequirement()
                    {
                        [jwtbearerScheme] = new string []{}
                    }
                };
            }
        }
    }
}

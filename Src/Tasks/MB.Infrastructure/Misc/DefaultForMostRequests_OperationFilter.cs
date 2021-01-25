using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Text;

namespace MB.Infrastructure.Misc
{
    public class DefaultForMostRequests_OperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Responses.TryAdd("400", new OpenApiResponse { Description = "Bad Request" });
            operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });
            operation.Responses.TryAdd("404", new OpenApiResponse { Description = "Not Found" });
        }
    }
}

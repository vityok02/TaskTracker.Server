using Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Infrastructure.Swagger;

public class ProblemDetailsSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(ProblemDetails))
        {
            schema.Properties["type"] = new OpenApiSchema
            {
                Type = "string",
                Enum = Enum.GetValues<ErrorType>()
                    .Cast<ErrorType>()
                    .Select(e => new OpenApiString(e.ToString()))
                    .ToList<IOpenApiAny>()
            };
        }
    }
}
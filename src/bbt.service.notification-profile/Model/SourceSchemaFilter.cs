using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Notification.Profile.Model;

public class SourceSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        schema.Example = new OpenApiObject
        {

            ["Title_TR"] = new OpenApiString(String.Empty),
            ["Title_EN"] = new OpenApiString(String.Empty),
            ["Topic"] = new OpenApiString(String.Empty),
            ["ApiKey"] = new OpenApiString(String.Empty),
            ["Secret"] = new OpenApiString(String.Empty),
            ["DisplayType"] = new OpenApiInteger(1),

            ["PushServiceReference"] = new OpenApiString(String.Empty),

            ["SmsServiceReference"] = new OpenApiString(String.Empty),

            ["EmailServiceReference"] = new OpenApiString(String.Empty),

            ["KafkaUrl"] = new OpenApiString(String.Empty),

            ["KafkaCertificate"] = new OpenApiString(String.Empty),

            ["ParentId"] = new OpenApiInteger(1),

            ["ClientIdJsonPath"] = new OpenApiString(String.Empty),

            ["RetentationTime"] = new OpenApiInteger(0),
        };
    }
}

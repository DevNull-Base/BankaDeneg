using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WalletAPI.Extensions;

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            schema.Enum.Clear();
            Enum.GetNames(context.Type)
                .ToList()
                .ForEach(name => schema.Enum.Add(new OpenApiString($"{GetEnumMemberValue(context.Type, name)}")));
        }
    }

    private string GetEnumMemberValue(System.Type enumType, string name)
    {
        return enumType.GetMember(name).First().GetCustomAttribute<EnumMemberAttribute>()?.Value ?? name;
    }
}
using System.Reflection;
using System.Runtime.Serialization;

namespace WalletAPI.Extensions;

public static class EnumExtensions
{
    public static T ParseEnum<T>(string value) where T : Enum
    {
        var enumType = typeof(T);
        foreach (var field in enumType.GetFields())
        {
            var attribute = field.GetCustomAttribute<EnumMemberAttribute>(false);
            if (attribute != null && attribute.Value == value)
            {
                return (T)field.GetValue(null);
            }
        }

        throw new ArgumentException($"Invalid value: {value}");
    }
}
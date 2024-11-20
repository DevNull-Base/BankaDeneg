using Newtonsoft.Json.Linq;

namespace WalletAPI.Extensions;

public static class JObjectExtensions
{
    public static JObject ConvertKeysToUpper(this JObject originalObject)
    {
        var newObject = new JObject();

        foreach (var property in originalObject.Properties())
        {
            var key = char.ToUpper(property.Name[0]) + property.Name.Substring(1);
            
            if (property.Value is JObject nestedObject)
            {
                newObject[key] = nestedObject.ConvertKeysToUpper();
            }
            else if (property.Value is JArray array)
            {
                newObject[key] = array.ConvertArrayKeysToUpper();
            }
            else
            {
                newObject[key] = property.Value;
            }
        }

        return newObject;
    }

    private static JArray ConvertArrayKeysToUpper(this JArray originalArray)
    {
        var newArray = new JArray();

        foreach (var item in originalArray)
        {
            if (item is JObject nestedObject)
            {
                newArray.Add(nestedObject.ConvertKeysToUpper());
            }
            else
            {
                newArray.Add(item);
            }
        }

        return newArray;
    }
}
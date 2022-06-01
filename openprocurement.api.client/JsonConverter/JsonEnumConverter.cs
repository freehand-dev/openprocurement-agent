using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.JsonConverter
{
    public class JsonEnumConverter<T> : JsonConverter<T> where T : struct, Enum, IComparable, IConvertible, IFormattable
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string propertyName = reader.GetString();
            return JsonEnumConverter<T>.GetEnumValue(propertyName);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {

            writer.WriteStringValue(
                JsonEnumConverter<T>.GetEnumMemberValue(value));    
        }


        public static String GetEnumMemberValue(Enum value)
        {
            EnumMemberAttribute attribute = value.GetType()
                            .GetField(value.ToString())
                            .GetCustomAttributes(typeof(EnumMemberAttribute), false)
                            .SingleOrDefault() as EnumMemberAttribute;

            return attribute == null ? value.ToString() : attribute.Value;
        }


        public static T GetEnumValue(string enumMemberText)
        {

            T retVal = default(T);

            if (Enum.TryParse<T>(enumMemberText, out retVal))
                return retVal;


            var enumVals = Enum.GetValues(typeof(T)).Cast<T>();

            Dictionary<string, T> enumMemberNameMappings = new Dictionary<string, T>();

            foreach (T enumVal in enumVals)
            {
                string enumMember = JsonEnumConverter<T>.GetEnumMemberValue(enumVal);
                enumMemberNameMappings.Add(enumMember, enumVal);
            }

            if (enumMemberNameMappings.ContainsKey(enumMemberText))
            {
                retVal = enumMemberNameMappings[enumMemberText];
            }
            else
                throw new SerializationException($"Could not resolve value {enumMemberText} in enum {typeof(T).FullName}");

            return retVal;
        }
    }
}

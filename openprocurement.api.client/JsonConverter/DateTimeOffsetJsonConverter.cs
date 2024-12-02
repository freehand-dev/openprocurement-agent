using System;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace openprocurement.api.client.JsonConverter
{
    public class DateTimeOffsetJsonConverter : JsonConverter<DateTimeOffset>
    {
        public static DateTimeOffset UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTimeStamp).ToUniversalTime();
        }

        public static double DateTimeToUnixTimeStamp(DateTimeOffset dateTime)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan span = (dateTime.ToUniversalTime() - epoch.ToUniversalTime());
            return span.TotalSeconds;
        }

        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) 
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Number:
                    return UnixTimeStampToDateTime(
                        Math.Floor(reader.GetDouble()));
                default:
                    var unixTimeString = reader.GetString();
                    return double.TryParse(unixTimeString, out double unixTime) ? DateTimeOffset.FromUnixTimeSeconds((long)Math.Floor(unixTime)) : DateTimeOffset.Parse(unixTimeString);          
            }     
        }
               
        public override void Write(
            Utf8JsonWriter writer,
            DateTimeOffset dateTimeValue,
            JsonSerializerOptions options) =>
                writer.WriteNumberValue(
                    DateTimeToUnixTimeStamp(dateTimeValue.ToUniversalTime()));
    }
}

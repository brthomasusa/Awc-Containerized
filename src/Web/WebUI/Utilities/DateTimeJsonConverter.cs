using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebUI.Utilities
{
    public class DateTimeJsonConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            string? dateString = reader.GetString();
            DateTime dateValue;
            DateTime.TryParse(dateString, out dateValue);
            return dateValue;

        }


        public override void Write(
            Utf8JsonWriter writer,
            DateTime dateTimeValue,
            JsonSerializerOptions options) =>
                writer.WriteStringValue(dateTimeValue.ToString(
                    "MM-dd-yyyy", CultureInfo.InvariantCulture));
    }
}
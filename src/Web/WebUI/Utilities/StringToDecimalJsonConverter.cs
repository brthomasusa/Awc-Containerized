using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebUI.Utilities
{
    public sealed class StringToDecimalJsonConverter : JsonConverter<decimal>
    {
        public override decimal Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            string? decimalString = reader.GetString();

            if (decimalString == null)
                return 0;

            return Decimal.Parse(decimalString);
        }

        public override void Write(
            Utf8JsonWriter writer,
            decimal decimalValue,
            JsonSerializerOptions options) =>
                writer.WriteStringValue(decimalValue.ToString(decimalValue.ToString(CultureInfo.InvariantCulture)));

    }
}
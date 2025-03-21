using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebUI.Utilities
{
    public class BitToBooleanJsonConverter : JsonConverter<bool>
    {
        public override bool Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) =>
                reader.GetString()! == "1";

        public override void Write(
            Utf8JsonWriter writer,
            bool boolValue,
            JsonSerializerOptions options) =>
                writer.WriteStringValue(true ? "1" : "0");

    }
}
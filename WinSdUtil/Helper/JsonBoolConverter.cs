using System.Text.Json;
using System.Text.Json.Serialization;

namespace WinSdUtil.Helper
{
    internal class JsonBoolConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
                reader.GetInt32() != 0;

        public override void Write(Utf8JsonWriter writer, bool boolValue, JsonSerializerOptions options) =>
                writer.WriteNumberValue(boolValue ? 1 : 0);

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(bool);
        }
    }
}

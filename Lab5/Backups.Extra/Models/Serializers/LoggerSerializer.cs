using System.Text.Json;
using System.Text.Json.Serialization;
using Backups.Extra.Interfaces;

namespace Backups.Extra.Models.Serializers;

public class LoggerSerializer : JsonConverter<ILogger>
{
    public override ILogger? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, ILogger value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.GetType().ToString());
    }
}
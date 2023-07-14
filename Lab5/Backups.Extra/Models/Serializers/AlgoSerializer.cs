using System.Text.Json;
using System.Text.Json.Serialization;
using Backups.Models.Algorithms;

namespace Backups.Extra.Models.Serializers;

public class AlgoSerializer : JsonConverter<IAlgorithm>
{
    public override IAlgorithm? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, IAlgorithm value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.GetType().ToString());
    }
}
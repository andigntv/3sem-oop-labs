using System.Text.Json;
using System.Text.Json.Serialization;
using Backups.Interfaces;

namespace Backups.Extra.Models.Serializers;

public class DirListSerializer : JsonConverter<IReadOnlyList<IDirectory>>
{
    public override IReadOnlyList<IDirectory>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, IReadOnlyList<IDirectory> value, JsonSerializerOptions options)
    {
        foreach (var directory in value)
            writer.WriteStringValue(directory.Path);
    }
}
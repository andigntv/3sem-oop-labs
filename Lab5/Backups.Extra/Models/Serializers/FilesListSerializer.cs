using System.Text.Json;
using System.Text.Json.Serialization;
using Backups.Interfaces;

namespace Backups.Extra.Models.Serializers;

public class FilesListSerializer : JsonConverter<IReadOnlyList<IFile>>
{
    public override List<IFile>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, IReadOnlyList<IFile> value, JsonSerializerOptions options)
    {
        foreach (var file in value)
            writer.WriteStringValue(file.Path);
    }
}
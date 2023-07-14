using System.Text.Json;
using System.Text.Json.Serialization;
using Backups.Interfaces;

namespace Backups.Extra.Models.Serializers;

public class FileSerializer : JsonConverter<IFile>
{
    public override IFile? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, IFile value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Path);
    }
}
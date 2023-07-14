using System.Text.Json;
using System.Text.Json.Serialization;
using Backups.Interfaces;

namespace Backups.Extra.Models.Serializers;

public class RepoSerializer : JsonConverter<IRepository>
{
    public override IRepository? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, IRepository value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.GetType().ToString());
    }
}
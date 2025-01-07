using System;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace PileRef.JsonConfig;

public class EncodingConverter : JsonConverter<Encoding>
{
    public override Encoding? ReadJson(JsonReader reader, Type objectType, Encoding? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        var encodingName = reader.ReadAsString();

        return encodingName != null ? Encoding.GetEncoding(encodingName) : null;
    }

    public override void WriteJson(JsonWriter writer, Encoding? value, JsonSerializer serializer)
    {
        writer.WriteValue(value?.WebName);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Héritage
{
    internal class MediaJsonConverter : JsonConverter<Media>
    {
        public override Media Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                JsonElement root = doc.RootElement;

                if (root.TryGetProperty("Author", out _))
                {
                    return JsonSerializer.Deserialize<Books>(root.GetRawText(), options);
                }
                else if (root.TryGetProperty("Director", out _))
                {
                    return JsonSerializer.Deserialize<DVD>(root.GetRawText(), options);
                }
                else
                {
                    throw new NotSupportedException("Type de média non supporté");
                }
            }
        }

        public override void Write(Utf8JsonWriter writer, Media value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}

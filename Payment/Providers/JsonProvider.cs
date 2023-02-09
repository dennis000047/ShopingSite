using System.Text.Json;
using System.Text.Json.Serialization;

namespace Payment.Providers
{
    public class JsonProvider
    {
        // JsonProvider 的序列化設定
        private JsonSerializerOptions serializeOption = new JsonSerializerOptions()
        {
            // 序列化過程將屬性名稱轉換為駝峰式命名
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            // 屬性名稱不區分大小寫
            PropertyNameCaseInsensitive = true,
            // 忽略 Null 值
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            // 使用 UnsafeRelaxedJsonEscapoing 編碼器來序列化 JSON
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        // JaonPeovider 的反序列化設定
        private static JsonSerializerOptions deserializeOptions = new JsonSerializerOptions
        {            
            PropertyNameCaseInsensitive = true,
        };

        public string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj, serializeOption);
        }

        public T Deserialize<T>(string str)
        {
            return JsonSerializer.Deserialize<T>(str, deserializeOptions);
        }
    }
}
using MongoDB.Bson;
using System.Text.Json.Serialization;
using ThirdParty.Json.LitJson;

namespace PostService.Dtos
{
    public class PostRedisDto
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("imageFile")]
        public byte[] ImageFile { get; set; }
    }
}

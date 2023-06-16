using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
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
        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        public PostRedisDto()
        {

        }

        public PostRedisDto(string id, string title, byte[] imageFile, string userId)
        {
            Id = id;
            Title = title;
            ImageFile = imageFile;
            UserId = userId;
        }
    }
}

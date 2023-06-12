using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace PostService.Models
{

    public class Post
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("title")]
        public string Title { get; set; }
        [BsonElement("imageFile")]
        public IFormFile ImageFile { get; set; }

        public Post(ObjectId id, string title)
        {
            Id = id;
            Title = title;
        }

        public Post(ObjectId id, string title, byte[] imageFile)
        {
            Id = id;
            Title = title;
            using (var memoryStream = new MemoryStream(imageFile))
            {
                ImageFile = new FormFile(memoryStream, 0, imageFile.Length, null, ".png");
            }
        }

        public Post()
        {
            
        }

        public Post(PostModel postModel)
        {
            Title = postModel.Title;
            using (var memoryStream = new MemoryStream(postModel.ImageFile))
            {
                ImageFile = new FormFile(memoryStream, 0, postModel.ImageFile.Length, null, ".png");
            }
        }

        public Post(string title, IFormFile imageFile)
        {
            Title = title;
            ImageFile = imageFile;
        }
    }
}

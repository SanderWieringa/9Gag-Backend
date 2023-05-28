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
        [BsonElement("photo")]
        public string Photo { get; set; }

       /* public Post(ObjectId id, string title, string photo)
        {
            Id = id;
            Title = title;
            Photo = photo;
        }*/
    }
}

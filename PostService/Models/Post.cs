﻿using MongoDB.Bson.Serialization.Attributes;
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

        public Post(string title, byte[] imageFile)
        {
            Title = title;
            using (var memoryStream = new MemoryStream(imageFile))
            {
                ImageFile = new FormFile(memoryStream, 0, imageFile.Length, null, ".png");
            }
        }

        public Post(string title, IFormFile imageFile)
        {
            Title = title;
            ImageFile = imageFile;
        }
    }
}

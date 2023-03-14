﻿using Backend.Models;
using System.Text.Json.Serialization;

namespace Backend.Dtos
{
    public class PostReadDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public IFormFile Photo { get; set; }
    }
}

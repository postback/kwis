using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Kwis.Models
{
    public class Quiz
    {
        public Quiz()
        {
        }

        [BsonId]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool Active { get; set; } = false;
        public bool Deleted { get; set; } = false;
        public bool Archived { get; set; } = false;
    }
}

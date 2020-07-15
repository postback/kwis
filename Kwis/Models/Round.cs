using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Kwis.Models
{
    public class Round
    {
        public Round()
        {
        }

        public Quiz Quiz { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public bool Active { get; set; }
    }
}

using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Kwis.Models
{
    public class Team
    {
        public Team()
        {
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}

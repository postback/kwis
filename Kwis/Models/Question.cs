using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Kwis.Models
{
    public class Question
    {
        public Question()
        {
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public Round Round { get; set; }

    }
}

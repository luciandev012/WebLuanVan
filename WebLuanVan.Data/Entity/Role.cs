using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebLuanVan.Data.Entity
{
    public class Role
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("roleName")]
        public string Name { get; set; }
    }
}

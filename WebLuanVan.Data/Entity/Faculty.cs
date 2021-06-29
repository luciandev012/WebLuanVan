using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebLuanVan.Data.Entity
{
    public class Faculty
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("facultyName")]
        public string FacultyName { get; set; }
        [BsonElement("facultyId")]
        public string FacultyId { get; set; }
        
        
    }
}

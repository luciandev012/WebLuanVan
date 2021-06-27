using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebLuanVan.Data.Entity
{
    public class Lecture
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("lectureId")]
        public string LectureId { get; set; }
        [BsonElement("lectureName")]
        public string LectureName { get; set; }
        [BsonElement("lectureRole")]
        public string LectureRole { get; set; }
        [BsonElement("email")]
        public string Email { get; set; }

    }
}

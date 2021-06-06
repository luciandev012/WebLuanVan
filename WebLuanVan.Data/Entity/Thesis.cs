using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebLuanVan.Data.Entity
{
    public class Thesis
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("thesisName")]
        public string ThesisName { get; set; }
        [BsonElement("studentId")]
        public string StudentId { get; set; }
        [BsonElement("year")]
        public int Year { get; set; }
        [BsonElement("phase")]
        public int Phase { get; set; }
        [BsonElement("academicYear")]
        public int AcademicYear { get; set; }
        [BsonElement("lectureId")]
        public string LectureId { get; set; }
        [BsonElement("facultyId")]
        public string FacultyId { get; set; }
        [BsonElement("content")]
        public string Content { get; set; }
        [BsonElement("thesisId")]
        public string ThesisId { get; set; }
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }
        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}

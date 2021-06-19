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
        [BsonElement("guideLectureId")]
        public string GuideLectureId { get; set; }
        [BsonElement("debateLectureId")]
        public List<string> DebateLectureId { get; set; }
        [BsonElement("facultyId")]
        public string FacultyId { get; set; }
        [BsonElement("content")]
        public string Content { get; set; }
        [BsonElement("thesisId")]
        public string ThesisId { get; set; }
        [BsonElement("protectedAt")]
        public DateTime? ProtectedAt { get; set; }
        [BsonElement("makedAt")]
        public DateTime? MakedAt { get; set; }
        [BsonElement("finishedAt")]
        public DateTime? FinishedAt { get; set; }
        [BsonElement("language")]
        public string Language { get; set; }
        [BsonElement("score")]
        public double Score { get; set; }
        [BsonElement("isProtected")]
        public bool IsProtected { get; set; }

    }
}

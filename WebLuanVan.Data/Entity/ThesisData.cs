using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebLuanVan.Data.Entity
{
    public class ThesisData
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("thesisId")]
        public string ThesisId { get; set; }
        [BsonElement("university")]
        public string University { get; set; }
        [BsonElement("thesisName")]
        public string ThesisName { get; set; }
        [BsonElement("summary")]
        public string Summary { get; set; }
        [BsonElement("chapter")]
        public string[] Chapter { get; set; }
        [BsonElement("pageCount")]
        public string PageCount { get; set; }
        [BsonElement("table")]
        public string[] Table { get; set; }
        [BsonElement("referDoc")]
        public string[] ReferDoc { get; set; }
        [BsonElement("comment")]
        public string Comment { get; set; }
        [BsonElement("recommend")]
        public string Recommend { get; set; }
        [BsonElement("student")]
        public string[] Student { get; set; }
        [BsonElement("class")]
        public string Class { get; set; }
        [BsonElement("academicYear")]
        public string AcademicYear { get; set; }
        [BsonElement("guideLecture")]
        public string GuideLecture { get; set; }
        [BsonElement("debateLecture")]
        public string[] DebateLecture { get; set; }
        [BsonElement("protectedDate")]
        public string ProtectedDate { get; set; }
    }
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebLuanVan.Data.Entity
{
    public class Student
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("facultyName")]
        public string FacultyName { get; set; }
        [BsonElement("facultyId")]
        public string FacultyId { get; set; }
        [BsonElement("phoneNumber")]
        public string PhoneNumber { get; set; }
        [BsonElement("studentName")]
        public string StudentName { get; set; }
        [BsonElement("studentCode")]
        public string StudentCode { get; set; }
        [BsonElement("class")]
        public string ClassName { get; set; }

    }
}

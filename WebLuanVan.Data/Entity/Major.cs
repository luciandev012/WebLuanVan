﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebLuanVan.Data.Entity
{
    public class Major
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("majorName")]
        public string MajorName { get; set; }
        [BsonElement("facultyId")]
        public string faculyId { get; set; }
    }
}

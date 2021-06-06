using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLuanVan.BackendApi
{
    public static class GetDatabase
    {
        private static IMongoDatabase database;
        //public GetDatabase(IMongoClient client)
        //{
        //    database = client.GetDatabase("manage_thesis");
        //}
        public static IMongoDatabase Get(IMongoClient client)
        {
            database = client.GetDatabase("manage_thesis");
            return database;
        }
    }
}

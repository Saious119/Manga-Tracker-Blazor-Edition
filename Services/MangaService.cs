using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaTracker_Temp.Services
{
    public class MangaService
    {
        List<Manga> mangaList = new List<Manga>();
        public async Task<List<Manga>> GetManga()
        {
            return mangaList;
        }
        public MangaService()
        {
            //Connect to MongoDB for Data

            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://guest:defaultPass@mangadb.hrhudi3.mongodb.net/?retryWrites=true&w=majority");
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            var database = client.GetDatabase("MangaDB");
            var collection = database.GetCollection<BsonDocument>("Manga");
            var documents = collection.Find(new BsonDocument()).ToList();
            foreach(BsonDocument doc in documents)
            {
                Console.WriteLine(doc.ToString());
            }
        }
        public async Task<List<BsonDocument>> GetMangaAsync()
        {
            //Connect to MongoDB for Data

            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://guest:defaultPass@mangadb.hrhudi3.mongodb.net/?retryWrites=true&w=majority");
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            var database = client.GetDatabase("MangaDB");
            var collection = database.GetCollection<BsonDocument>("Manga");
            var documents = collection.Find(new BsonDocument()).ToList();
            foreach (BsonDocument doc in documents)
            {
                Console.WriteLine(doc.ToString());
            }
            return documents;
        }
    }
}

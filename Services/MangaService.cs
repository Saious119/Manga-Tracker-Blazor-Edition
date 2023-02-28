using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MangaTracker_Temp.Services
{
    public class MangaService
    {
        List<Manga> mangaList = new List<Manga>();
        List<int> avgs = new List<int>(); 
        public async Task<List<Manga>> GetManga()
        {
            return mangaList;
        }
        public MangaService()
        {
            //Connect to MongoDB for Data

            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://guest:defaultPass@serverlessinstance.izekv.mongodb.net/?retryWrites=true&w=majority");
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            var database = client.GetDatabase("MangaDB");
            //var collection = database.GetCollection<BsonDocument>(user);
            //var documents = collection.Find(new BsonDocument()).ToList();
            /*foreach (BsonDocument doc in documents)
            {
                Console.WriteLine(doc.ToString());
            }*/
        }
        public async Task<List<BsonDocument>> GetMangaAsync(string user)
        {
            if(user == null)
            {
                user = "NoUser";
            }
            //Connect to MongoDB for Data
            Console.WriteLine("here!!");
            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://guest:defaultPass@serverlessinstance.izekv.mongodb.net/?retryWrites=true&w=majority");
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            var database = client.GetDatabase("MangaDB");
            IMongoCollection<BsonDocument> collection = null;
            Console.WriteLine("checking for user");
            collection = database.GetCollection<BsonDocument>(user);
            if (collection == null)
            {
                Console.WriteLine("making new User {0}", user);
                await database.CreateCollectionAsync(user);
            }
            collection = database.GetCollection<BsonDocument>(user);
            Console.WriteLine("got collection");
            var documents = collection.Find(new BsonDocument()).ToList();
            foreach (BsonDocument doc in documents)
            {
                Console.WriteLine(doc.ToString());
            }
            return documents;
        }
        public async Task AddMangaToDB(Manga newManga, string user)
        {
            try
            {
                var settings = MongoClientSettings.FromConnectionString("mongodb+srv://guest:defaultPass@serverlessinstance.izekv.mongodb.net/?retryWrites=true&w=majority");
                settings.ServerApi = new ServerApi(ServerApiVersion.V1);
                var client = new MongoClient(settings);
                var database = client.GetDatabase("MangaDB");
                var collection = database.GetCollection<BsonDocument>(user);
                var documents = collection.Find(new BsonDocument()).ToList();
                var newDoc = newManga.ToBsonDocument();
                collection.InsertOne(newDoc);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public async Task RemoveManga(string nameToFind, string user)
        {
            try
            {
                var settings = MongoClientSettings.FromConnectionString("mongodb+srv://guest:defaultPass@serverlessinstance.izekv.mongodb.net/?retryWrites=true&w=majority");
                settings.ServerApi = new ServerApi(ServerApiVersion.V1);
                var client = new MongoClient(settings);
                var database = client.GetDatabase("MangaDB");
                var collection = database.GetCollection<BsonDocument>(user);
                var deleteFilter = Builders<BsonDocument>.Filter.Eq("Name", nameToFind);
                collection.DeleteOne(deleteFilter);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public async Task UpdateManga(Manga mangaToUpdate, string user)
        {
            try
            {
                await RemoveManga(mangaToUpdate.Name, user);
                await AddMangaToDB(mangaToUpdate, user);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public IMongoCollection<BsonDocument>? GetCollection(string user)
        {
            try
            {
                var settings = MongoClientSettings.FromConnectionString("mongodb+srv://guest:defaultPass@serverlessinstance.izekv.mongodb.net/?retryWrites=true&w=majority");
                settings.ServerApi = new ServerApi(ServerApiVersion.V1);
                var client = new MongoClient(settings);
                var database = client.GetDatabase("MangaDB");
                var collection = database.GetCollection<BsonDocument>(user);
                return collection;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }
        public string CalcCompletion(int numRead, int numVolumes) 
        {
            var val = Math.Truncate(((double)numRead / (double)numVolumes) * 100).ToString();
            if(val == null)
            {
                return "NaN";
            }
            avgs.Add(Int32.Parse(val));
            return val.ToString();
        }
        public string AvgCalc()
        {
            int total = 0;
            foreach(var item in avgs)
            {
                total += item;
            }
            if(total == 0 || avgs.Count() == 0)
            {
                return string.Empty;
            }
            total = total / avgs.Count();
            return total.ToString();
        }
    }
}

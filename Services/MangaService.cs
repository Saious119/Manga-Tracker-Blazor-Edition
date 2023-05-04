using MongoDB.Bson;

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
        }
        public async Task<List<Manga>> GetMangaAsync(string user)
        {
            if(user == null)
            {
                user = "NoUser";
            }
            //Connect to MongoDB for Data
            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://guest:defaultPass@serverlessinstance.izekv.mongodb.net/?retryWrites=true&w=majority");
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            var database = client.GetDatabase("MangaDB");
            IMongoCollection<Manga> collection = null;
            Console.WriteLine("Checking DB for user: {0}", user);
            collection = database.GetCollection<Manga>(user);
            if (collection == null)
            {
                Console.WriteLine("Making new User {0}", user);
                await database.CreateCollectionAsync(user);
            }
            collection = database.GetCollection<Manga>(user);
            Console.WriteLine("Found collection for user: {0}", user);
            var documents = collection.Find(new BsonDocument()).ToList();
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
        public IMongoCollection<Manga>? GetCollection(string user)
        {
            try
            {
                var settings = MongoClientSettings.FromConnectionString("mongodb+srv://guest:defaultPass@serverlessinstance.izekv.mongodb.net/?retryWrites=true&w=majority");
                settings.ServerApi = new ServerApi(ServerApiVersion.V1);
                var client = new MongoClient(settings);
                var database = client.GetDatabase("MangaDB");
                var collection = database.GetCollection<Manga>(user);
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
            try
            {
                int total = 0;
                foreach (var item in avgs)
                {
                    total += item;
                }
                if (total == 0 || avgs.Count() == 0)
                {
                    return string.Empty;
                }
                total = total / avgs.Count();
                return total.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return "0";
        }
    }
}

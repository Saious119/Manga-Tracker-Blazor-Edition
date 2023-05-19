using MongoDB.Bson;
using log4net;

namespace MangaTracker_Temp.Services
{
    public class MangaService
    {
        List<Manga> mangaList = new List<Manga>();
        List<int> avgs = new List<int>();
        private ILog log = LogManager.GetLogger(typeof(Program));
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
            log.Info("Checking DB for user: "+ user);
            collection = database.GetCollection<Manga>(user);
            if (collection == null)
            {
                log.Info("Making new User "+ user);
                await database.CreateCollectionAsync(user);
            }
            collection = database.GetCollection<Manga>(user);
           log.Info("Found collection for user: "+ user);
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
                log.Error(e);
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
                log.Error(e);
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
                log.Error(e);
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
                log.Error(e);
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
                log.Error(e);
            }
            return "0";
        }
    }
}

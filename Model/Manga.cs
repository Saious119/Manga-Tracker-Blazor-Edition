using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaTracker_Temp.Model
{
    public class Manga
    {
        [BsonId]
        public ObjectId id { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("Author")]
        public string Author { get; set; }
        [BsonElement("numVolumes")]
        public string numVolumes { get; set; }
        [BsonElement("numRead")]
        public string numRead { get; set; }
    }
}

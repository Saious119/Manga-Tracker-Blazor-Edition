using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaTracker_Temp.Model
{
    public class Manga
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string numRead { get; set; }
        public string numVolumes { get; set; }

        public bool isUpdateFormVisible = false;
        public Manga(string Name, string Author, string numRead, string numVolumes)
        {
            this.Name = Name;
            this.Author = Author;
            this.numRead = numRead;
            this.numVolumes = numVolumes;
        }
        public Manga() { }
    }
}

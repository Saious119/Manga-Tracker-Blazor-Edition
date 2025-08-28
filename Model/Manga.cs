using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaTracker_Temp.Model
{
    public class Manga
    {
        [Key]
        public int rowid { get; set; }
        public string owner { get; set; }
        public string? name { get; set; }
        public string? author { get; set; }
        public string? numread { get; set; }
        public string? numvolumes { get; set; }
        [NotMapped]
        public bool isUpdateFormVisible = false;
        public Manga(string name, string author, string numread, string numvolumes)
        {
            this.name = name;
            this.author = author;
            this.numread = numread;
            this.numvolumes = numvolumes;
        }
        public Manga() { }
    }
}

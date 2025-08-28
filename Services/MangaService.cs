using log4net;
using MangaTracker_Temp.Data;
using Npgsql;

namespace MangaTracker_Temp.Services
{
    public class MangaService : IMangaService
    {
        List<Manga> mangaList = new List<Manga>();
        List<int> avgs = new List<int>();
        private ILog log = LogManager.GetLogger(typeof(Program));
        private string connString; 
        private readonly IConfiguration _configuration;
        private DataContext _context;

        public MangaService(IConfiguration configuration, DataContext context)
        {
            this._configuration = configuration;
            this._context = context;
        }
        public async Task<List<Manga>> GetManga()
        {
            return mangaList;
        }
        public List<Manga> GetManga(string user)
        {
            if(user == null)
            {
                user = "NoUser";
            }
            List<Manga> listToReturn = new List<Manga>();
            try
            {
                listToReturn = _context.Manga.Where(x => x.owner == user).ToList();
            } catch(Exception e) {
                Console.WriteLine(e);
            }
            return listToReturn;
        }
        public async Task AddMangaToDB(Manga newManga, string user)
        {
            try
            {
                newManga.owner = user;
                _context.Manga.Add(newManga);
                await _context.SaveChangesAsync();
            }
            catch(Exception e) { Console.WriteLine(e); }
        }
        public async Task RemoveManga(string nameToFind, string authorToFind, string user)
        {
            try
            {
                var mangaToRemove = _context.Manga.FirstOrDefault(m => m.name == nameToFind && m.author == authorToFind && m.owner == user);
                if (mangaToRemove != null)
                {
                    _context.Manga.Remove(mangaToRemove);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e) { Console.WriteLine(e); }
        }
        public async Task UpdateManga(Manga mangaToUpdate, string user)
        {
            try
            {
                var manga = _context.Manga.FirstOrDefault(m => m.name == mangaToUpdate.name && m.author == mangaToUpdate.author && m.owner == user);
                if (manga != null)
                {
                    manga.name = mangaToUpdate.name;
                    manga.author = mangaToUpdate.author;
                    manga.numread = mangaToUpdate.numread;
                    manga.numvolumes = mangaToUpdate.numvolumes;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public string CalcCompletion(int numread, int numvolumes) 
        {
            var val = Math.Truncate(((double)numread / (double)numvolumes) * 100).ToString();
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
                if (total == 0 || avgs.Count == 0)
                {
                    return "0";
                }
                total = total / avgs.Count;
                return total.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return "0";
        }
        public string AvgCalc(List<Manga> _mangaList)
        {
            try
            {
                int total = 0;
                foreach (var item in _mangaList)
                {
                    var seriesCompletion = CalcCompletion(Int32.Parse(item.numread), Int32.Parse(item.numvolumes));
                    total += Int32.Parse(seriesCompletion);
                }
                if (total == 0 || _mangaList.Count() == 0)
                {
                    return "0";//string.Empty;
                }
                total = total / _mangaList.Count;
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

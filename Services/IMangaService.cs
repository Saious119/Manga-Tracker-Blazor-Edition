namespace MangaTracker_Temp.Services
{
    public interface IMangaService
    {
        public Task<List<Manga>> GetManga();
        public List<Manga> GetManga(string user);
        public Task AddMangaToDB(Manga newManga, string user);
        public Task RemoveManga(string nameToFind, string authorToFind, string user);
        public Task UpdateManga(Manga mangaToUpdate, string user);
        public string CalcCompletion(int numRead, int numVolumes);
        public string AvgCalc();
        public string AvgCalc(List<Manga> _mangaList);
    }
}

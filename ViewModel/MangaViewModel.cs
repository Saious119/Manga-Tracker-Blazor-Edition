using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MangaTracker_Temp.Services;
using Microsoft.Graph;
using MvvmBlazor;
using log4net;

namespace MangaTracker_Temp.ViewModel
{
    public partial class MangaViewModel : BaseViewModel
    {
        public ObservableCollection<Manga> MangaList { get; } = new();
        public AsyncRelayCommand GetMangaCommand { get; }
        MangaService mangaService;
        private ILog log = LogManager.GetLogger(typeof(Program));
        public MangaViewModel()
        {
            Title = "Manga Tracker";
            this.mangaService = mangaService;
            GetMangaCommand = new AsyncRelayCommand(async () => await GetMangaAsync());
        }
        async Task GetMangaAsync()
        {
            if (IsBusy)
            {
                return;
            }
            try
            {
                IsBusy = true;
                var mangas = await mangaService.GetManga();
                if(MangaList.Count != 0)
                {
                    MangaList.Clear();
                }
                foreach(var manga in mangas)
                {
                    MangaList.Add(manga);
                }
            }
            catch(Exception e) { 
                log.Error(e.Message);
                //await Shell.Current.DisplayAlert("Error!", e.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

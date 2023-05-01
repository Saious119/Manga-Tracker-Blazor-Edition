namespace MangaTracker_Temp.Services
{
    public class DiscordService
    {
        public string botToken { get; set; }
        public string clientID { get; set; }
        public string clientSecret { get; set; }
        public DiscordService() 
        {
            var lines = File.ReadAllLines("DiscordAuth.txt");
            botToken = lines[0];
            clientID = lines[1];
            clientSecret = lines[2];
        }
    }
}

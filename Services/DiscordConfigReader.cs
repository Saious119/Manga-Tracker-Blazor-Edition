namespace MangaTracker_Temp.Services
{
    public class DiscordConfigReader : IDiscordConfigReader
    {
        private readonly IConfiguration _configuration;
        public static string? botToken { get; set; }
        public static string? clientID { get; set; }
        public static string? clientSecret { get; set; }
        public static string? userDiscordID { get; set; }
        public DiscordConfigReader(IConfiguration configuration) 
        {
            this._configuration = configuration;
            botToken = this._configuration["discord_bot_token"];
            clientID = this._configuration["discord_client_id"];
            clientSecret = this._configuration["discord_client_secret"];
        }
    }
}

namespace MangaTracker_Temp.Services
{
    public class DiscordConfigReader : IDiscordConfigReader
    {
        private readonly IConfiguration _configuration;
        public string? botToken { get; set; }
        public string? clientID { get; set; }
        public string? clientSecret { get; set; }
        public string? userDiscordID { get; set; }
        public DiscordConfigReader(IConfiguration configuration) 
        {
            this._configuration = configuration;
            botToken = this._configuration["discord_bot_token"];
            clientID = this._configuration["discord_client_id"];
            clientSecret = this._configuration["discord_client_secret"];
        }

        public string getToken()
        {
            if (this.botToken == null)
            {
                return "dummyToken";
            }
            return this.botToken;
        }
    }
}

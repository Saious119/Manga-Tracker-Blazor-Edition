namespace MangaTracker_Temp
{
    public class StaticVariables
    {
        private readonly IConfiguration _configuration;
        public StaticVariables(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        public static string? DB_CONNECT_STRING => Environment.GetEnvironmentVariable("db_connect_string");
        public static string? ClientId => Environment.GetEnvironmentVariable("discord_client_id");
        public static string? ClientSecret => Environment.GetEnvironmentVariable("discord_client_secret");
        public static string? BotToken => Environment.GetEnvironmentVariable("discord_bot_token");
        public static string? SendGridKey => Environment.GetEnvironmentVariable("send_grid_key");
    }
}

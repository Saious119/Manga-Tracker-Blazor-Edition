namespace MangaTracker_Temp
{
    public class StaticVariables
    {
        public static string? DB_CONNECT_STRING => Environment.GetEnvironmentVariable("db_connect_string");
    }
}

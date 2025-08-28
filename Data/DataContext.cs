using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace MangaTracker_Temp.Data
{
    public class DataContext : DbContext
    {
        private IConfiguration _configuration;

        public DataContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionStringBuilder());
        }

        public DbSet<Manga> Manga { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manga>().ToTable("manga", "public");
        }
        public string ConnectionStringBuilder()
        {
            var connStringBuilder = new NpgsqlConnectionStringBuilder();
            connStringBuilder.SslMode = SslMode.Require;
            connStringBuilder.TrustServerCertificate = true;
            string databaseUrlEnv = this._configuration["db_connect_string"];
            if (databaseUrlEnv == null)
            {
                Console.WriteLine("Setting DB to Localhost");
                connStringBuilder.Host = "localhost";
                connStringBuilder.Port = 26257;
                connStringBuilder.Username = "username";
                connStringBuilder.Passfile = "password";
                connStringBuilder.IncludeErrorDetail = true;
            }
            else
            {
                Console.WriteLine("Found Environment DB Connect String");
                Uri databaseUrl = new Uri(databaseUrlEnv);
                connStringBuilder.Host = databaseUrl.Host;
                connStringBuilder.Port = databaseUrl.Port;
                var items = databaseUrl.UserInfo.Split(new[] { ':' });
                if (items.Length > 0) { connStringBuilder.Username = items[0]; }
                if (items.Length > 1) { connStringBuilder.Password = items[1]; }
                connStringBuilder.IncludeErrorDetail = true;
            }
            connStringBuilder.Database = "mangadb";
            return connStringBuilder.ToString();
        }
    }
}

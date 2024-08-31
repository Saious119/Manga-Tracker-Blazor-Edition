using log4net;
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

        public MangaService(IConfiguration configuration)
        {
            this._configuration = configuration;
            var connStringBuilder = new NpgsqlConnectionStringBuilder();
            connStringBuilder.SslMode = SslMode.VerifyFull;
            string? databaseUrlEnv = this._configuration["db_connect_string"];
            if (databaseUrlEnv == "")
            {
                connStringBuilder.Host = "localhost";
                connStringBuilder.Port = 26257;
                connStringBuilder.Username = "username";
                connStringBuilder.Passfile = "password";
                connStringBuilder.IncludeErrorDetail = true;
            }
            else
            {
                Uri databaseUrl = new Uri(databaseUrlEnv);
                connStringBuilder.Host = databaseUrl.Host;
                connStringBuilder.Port = databaseUrl.Port;
                var items = databaseUrl.UserInfo.Split(new[] { ':' });
                if (items.Length > 0) { connStringBuilder.Username = items[0]; }
                if (items.Length > 1) { connStringBuilder.Password = items[1]; }
                connStringBuilder.IncludeErrorDetail = true;
            }
            connStringBuilder.Database = "mangadb";
            connString = connStringBuilder.ConnectionString;
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
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    string accountSqlCmd = "CREATE TABLE IF NOT EXISTS " + user + " (name VARCHAR, author VARCHAR, numread VARCHAR, numvolumes VARCHAR)";
                    using (var cmd = new NpgsqlCommand(accountSqlCmd, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    string sql = "SELECT name, author, numread, numvolumes FROM " + user;
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //Console.WriteLine("{0} by {1}, {2}/{3}", reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3));
                                Manga MangaToReturn = new Manga(reader.GetValue(0).ToString(), reader.GetValue(1).ToString(), reader.GetValue(2).ToString(), reader.GetValue(3).ToString());
                                if (MangaToReturn != null)
                                {
                                    listToReturn.Add(MangaToReturn);
                                }
                            }
                        }
                    }
                }
            } catch(Exception e) {
                Console.WriteLine(e);
            }
            return listToReturn;
        }
        public async Task AddMangaToDB(Manga newManga, string user)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "UPSERT INTO " + user + "(name, author, numread, numvolumes) VALUES(@val1, @val2, @val3, @val4)";
                        cmd.Parameters.AddWithValue("val1", newManga.Name);
                        cmd.Parameters.AddWithValue("val2", newManga.Author);
                        cmd.Parameters.AddWithValue("val3", newManga.numRead);
                        cmd.Parameters.AddWithValue("val4", newManga.numVolumes);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch(Exception e) { Console.WriteLine(e); }
        }
        public async Task RemoveManga(string nameToFind, string authorToFind, string user)
        {
            try
            {
                using(var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    using(var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "DELETE FROM " + user + " WHERE name='" + nameToFind + "' AND author='" + authorToFind+"'";
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e); }
        }
        public async Task UpdateManga(Manga mangaToUpdate, string user)
        {
            try
            {
                await RemoveManga(mangaToUpdate.Name, mangaToUpdate.Author, user);
                await AddMangaToDB(mangaToUpdate, user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public string CalcCompletion(int numRead, int numVolumes) 
        {
            var val = Math.Truncate(((double)numRead / (double)numVolumes) * 100).ToString();
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
                if (total == 0 || avgs.Count() == 0)
                {
                    return "0";
                }
                total = total / avgs.Count();
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
                    var seriesCompletion = CalcCompletion(Int32.Parse(item.numRead), Int32.Parse(item.numVolumes));
                    total += Int32.Parse(seriesCompletion);
                }
                if (total == 0 || _mangaList.Count() == 0)
                {
                    return "0";//string.Empty;
                }
                total = total / _mangaList.Count();
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

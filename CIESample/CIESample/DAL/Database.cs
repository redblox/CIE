using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using System.Threading.Tasks;

namespace CIESample.DAL
{
    class Database
    {
        private static string MAIN_DB_PATH = "CIE_DATA_DB.sqlite";

        public static string MainDBPath
        {
            get
            {
                #if __ANDROID__
                // Just use whatever directory SpecialFolder.Personal returns
                string libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                //string libraryPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
                #else
                // we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
                // (they don't want non-user-generated data in Documents)
                string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
                string libraryPath = Path.Combine (documentsPath, "..", "Library"); // Library folder
                #endif
                var path = Path.Combine(libraryPath, MAIN_DB_PATH);
                Console.WriteLine(path + " path");
                return path;

            }
        }


        public static void InitMainDB(bool overwrite = false)
        {
            try
            {
                File.Create(MainDBPath);
                using (var conn = new SQLite.SQLiteConnection(MainDBPath, SQLite.SQLiteOpenFlags.ReadWrite | SQLite.SQLiteOpenFlags.Create, false))
                {
                    var types = new[]
                    {
                        typeof(Models.Favorites)
                    };
                    foreach (var type in types)
                    {
                        conn.CreateTable(type);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }



        public static SQLite.SQLiteConnection GetSyncConnection()
        {
            if (File.Exists(MainDBPath))
            {
                return new SQLite.SQLiteConnection(MainDBPath, SQLite.SQLiteOpenFlags.ReadWrite | SQLite.SQLiteOpenFlags.Create);
            }
            else
            {
                InitMainDB();
                return new SQLite.SQLiteConnection(MainDBPath, SQLite.SQLiteOpenFlags.ReadWrite | SQLite.SQLiteOpenFlags.Create);
            }
        }



        public static async Task<string> StroreFav(string title, string filmoverview, string posterPath, string releaseDate, string passedID)
        {
                using (var conn = GetSyncConnection())
                {
                    conn.Insert(new Models.Favorites()
                    {
                        original_title = title,
                        overview = filmoverview,
                        poster_path = posterPath,
                        release_date = releaseDate,
                        Id = Convert.ToInt32(passedID)
                    });
                    return null;
                }
        }


        public static async Task<List<Models.Favorites>> GetFavFilms()
        {
            return await Task.Run(() =>
            {
                using (var conn = GetSyncConnection())
                {
                    return conn.Table<Models.Favorites>().ToList();
                }
            });
        }


        public static async Task deleteFavFilm(int idPassed)
        {
            await Task.Run(() =>
            {
                using (var conn = GetSyncConnection())
                {
                    var statuses = conn.Table<Models.Favorites>().Where(o => o.Id == idPassed).FirstOrDefault();
                    conn.Delete(statuses);                    
                }
            });
        }

    }
}
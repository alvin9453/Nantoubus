using Mono.Data.Sqlite;
using Nantou_bus.Droid;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_Android))]
namespace Nantou_bus.Droid
{
    public class SQLite_Android : ISQLite
    {
        public SQLite_Android()
        {
        }

        #region ISQLite implementation
        public SQLite.SQLiteConnection GetConnection()
        {
            var sqliteFilename = "LocalDatabase.cs";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);

            var conn = new SQLite.SQLiteConnection(path);

            // Return the database connection 
            return conn;
        }
        public SqliteConnection GetDatabaseConnection()
        {
            string path = FileAccessHelper.GetLocalFilePath("ibus_v7.db3");
            SqliteConnection conn = new SqliteConnection("URI="+path);
            return conn;
        }
        #endregion
    }
}
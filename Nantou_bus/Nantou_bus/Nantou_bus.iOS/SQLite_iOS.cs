using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;
using Nantou_bus.iOS;
using Mono.Data.Sqlite;

[assembly: Dependency(typeof(SQLite_iOS))]
namespace Nantou_bus.iOS
{
    public class SQLite_iOS : ISQLite
    {
        public SQLite_iOS()
        {
        }

        #region ISQLite implementation
        public SQLite.SQLiteConnection GetConnection()
        {
            var sqliteFilename = "LocalDatabase.cs";
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            var path = Path.Combine(libraryPath, sqliteFilename);

            var conn = new SQLite.SQLiteConnection(path);

            // Return the database connection 
            return conn;
        }
        #endregion
        public SqliteConnection GetDatabaseConnection()
        {
            string path = FileAccessHelper.GetLocalFilePath("ibus_v7.db3");
            SqliteConnection conn = new SqliteConnection("URI=" + path);
            return conn;
        }
    }
}
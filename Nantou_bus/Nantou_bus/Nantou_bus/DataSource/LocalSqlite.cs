using Mono.Data.Sqlite;
using Xamarin.Forms;

namespace Nantou_bus.DataSource
{
    public class LocalSqlite : AbstractDataSource<SqliteDataReader>
    {
        public override SqliteDataReader Open(string sql)
        {
            Conn = DependencyService.Get<ISQLite>().GetDatabaseConnection();
            Command = new SqliteCommand(sql, (SqliteConnection)Conn);
            ((SqliteConnection)Conn).Open();
            SqliteDataReader dataReader = ((SqliteCommand)Command).ExecuteReader();
            return dataReader;
        }
    }
}

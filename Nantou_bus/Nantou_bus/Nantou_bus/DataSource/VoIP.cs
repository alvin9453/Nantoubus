using MySql.Data.MySqlClient;
using System.Data.Common;

namespace Nantou_bus.DataSource
{
    public class VoIP : AbstractDataSource<MySqlDataReader>
    {
        private const string ConnectionString = "Server=ms14.voip.edu.tw;Port=3306;database=ibus;User ID=ibus;Password=ibus;charset=utf8";

        public override MySqlDataReader Open(string sql)
        {
            Conn = new MySqlConnection(ConnectionString);
            Command = new MySqlCommand(sql, (MySqlConnection)Conn);
            ((DbConnection)Conn).Open();
            MySqlDataReader dataReader = ((MySqlCommand)Command).ExecuteReader();
            return dataReader;
        }
    }
}

using MySql.Data.MySqlClient;
using Nantou_bus.DataSource;

namespace Nantou_bus.Repository
{
    public abstract class AbstractMysqlRepository<T> : AbstractGenericRepository<T, MySqlDataReader>
    {
        private AbstractDataSource<MySqlDataReader> DataSource;

        protected override AbstractDataSource<MySqlDataReader> GetDataSource()
        {
            if (DataSource == null) { DataSource = new VoIP(); }
            return DataSource;
        }
    }
}

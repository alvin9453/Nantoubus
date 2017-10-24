using Mono.Data.Sqlite;
using Nantou_bus.DataSource;

namespace Nantou_bus.Repository
{
    public abstract class AbstractSQLiteRepository<T> : AbstractGenericRepository<T, SqliteDataReader>
    {
        private AbstractDataSource<SqliteDataReader> DataSource;

        protected override AbstractDataSource<SqliteDataReader> GetDataSource()
        {
            if (DataSource == null) { DataSource = new LocalSqlite(); }
            return DataSource;
        }
    }
}

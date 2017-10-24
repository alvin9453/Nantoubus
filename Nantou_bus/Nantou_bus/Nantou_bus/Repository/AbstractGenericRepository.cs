using Nantou_bus.DataSource;
using System.Collections.Generic;
using System.Data.Common;

namespace Nantou_bus.Repository
{
    public abstract class AbstractGenericRepository<T1, T2> where T2 : DbDataReader
    {
        protected IEnumerable<T1> Query(string sql)
        {
            AbstractDataSource<T2> datasource = GetDataSource();
            try
            {
                T2 reader = datasource.Open(sql);
                IList<T1> results = new List<T1>();
                while (reader.Read())
                {
                    T1 entity = ToEntity(reader);
                    results.Add(entity);
                }
                return results;
            }
            finally { datasource.Close(); }
        }

        protected abstract AbstractDataSource<T2> GetDataSource();

        protected abstract T1 ToEntity(T2 reader);
    }
}

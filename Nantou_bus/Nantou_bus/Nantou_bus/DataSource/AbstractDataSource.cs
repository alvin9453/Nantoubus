using System;
using System.Data.Common;

namespace Nantou_bus.DataSource
{
    public abstract class AbstractDataSource<T> where T : DbDataReader
    {
        public IDisposable Conn { get; protected set; }

        public DbCommand Command { get; protected set; }

        public abstract T Open(string sql);

        public void Close()
        {
            if (Conn != null) { Conn.Dispose(); }
            if (Command != null) { Command.Dispose(); }
        }
    }
}

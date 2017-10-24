using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mono.Data.Sqlite;

namespace Nantou_bus
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
        SqliteConnection GetDatabaseConnection();
    }
}
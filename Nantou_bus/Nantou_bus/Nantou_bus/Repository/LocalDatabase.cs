using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

using Nantou_bus.Model;

namespace Nantou_bus
{
    public class LocalDatabase
    {
        private SQLiteConnection Connection;

        private static object Mutex = new object();

        private string Path { get; set; }

        public LocalDatabase()
        {
            Connection = DependencyService.Get<ISQLite>().GetConnection();
            Path = Connection.DatabasePath;
            Connection.CreateTable<MyRecord>();
            Connection.CreateTable<HistoryRecord>();
            Connection.CreateTable<OfficeRecord>();
            Connection.CreateTable<SchoolRecord>();
        }

        public void Clear<T>() where T : AbstractRecord, new()
        {
            IEnumerable<T> items = List<T>();
            foreach (T item in items) { Delete<T>(item.ID); }
        }

        public int Create<T>(T item) where T : AbstractRecord
        {
            lock (Mutex)
            {
                if (item.ID == 0) { return Connection.Insert(item); }
                Connection.Update(item);
                return item.ID;
            }
        }

        public int Delete<T>(int id) where T : AbstractRecord
        {
            lock (Mutex) { return Connection.Delete<T>(id); }
        }

        public T Get<T>(int id) where T : AbstractRecord, new()
        {
            lock (Mutex) { return Connection.Table<T>().FirstOrDefault(x => x.ID == id); }
        }

        public List<T> List<T>() where T : AbstractRecord, new()
        {
            lock (Mutex) { return (from i in Connection.Table<T>() select i).ToList(); }
        }
    }
}
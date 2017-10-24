using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Linq;

using Nantou_bus.Model;

namespace Nantou_bus.Repository
{
    public class StopOnRouteSQLiteRepository : AbstractSQLiteRepository<StopOnRoute>
    {
        public static StopOnRouteSQLiteRepository Instance
        {
            get
            {
                if (_Instance == null) { _Instance = new StopOnRouteSQLiteRepository(); }
                return _Instance;
            }
        }

        private static StopOnRouteSQLiteRepository _Instance;

        private const string SQL_LIST = "SELECT RouteUID, SubRouteUID, Direction, StopID FROM `bus_stops_on_route` WHERE StopID LIKE '%{0}%' order by SubRouteUID";

        public IEnumerable<StopOnRoute> Retrieve(IEnumerable<string> stopIDs)
        {
            IEnumerable<StopOnRoute> results = new List<StopOnRoute>();
            foreach (string stopID in stopIDs) { results = results.Concat(Retrieve(stopID)); }
            return results;
        }

        public IEnumerable<StopOnRoute> Retrieve(string stopID)
        {
            IEnumerable<StopOnRoute> results = Query(string.Format(SQL_LIST, stopID));
            return results;
        }

        protected override StopOnRoute ToEntity(SqliteDataReader reader)
        {
            StopOnRoute entity = new StopOnRoute();
            entity.RouteUID = reader.GetString(0);
            entity.SubRouteUID = reader.GetString(1);
            entity.Direction = reader.GetBoolean(2);
            entity.StopID = reader.GetString(3);
            return entity;
        }
    }
}

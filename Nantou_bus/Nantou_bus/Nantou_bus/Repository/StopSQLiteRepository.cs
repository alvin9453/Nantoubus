using System;
using System.Collections.Generic;

using Mono.Data.Sqlite;
using Nantou_bus.Model;

namespace Nantou_bus.Repository
{
    public class StopSQLiteRepository : AbstractSQLiteRepository<Stop>
    {
        public static StopSQLiteRepository Instance
        {
            get
            {
                if (_Instance == null) { _Instance = new StopSQLiteRepository(); }
                return _Instance;
            }
        }

        private const string SQL_GET = "SELECT StopID, StopName, LAT, LON FROM bus_stops GROUP BY StopName";

        private const string SQL_GET_BY_NAME = "SELECT StopID, StopName, LAT, LON FROM bus_stops WHERE StopName LIKE '%{0}%'";

        private static StopSQLiteRepository _Instance;

        public IEnumerable<Stop> List()
        {
            IEnumerable<Stop> results = Query(SQL_GET);
            return results;
        }

        public IEnumerable<Stop> Get(String stopName)
        {
            string sql = string.Format(SQL_GET_BY_NAME, stopName);
            IEnumerable<Stop> results = Query(sql);
            return results;
        }

        protected override Stop ToEntity(SqliteDataReader reader)
        {
            Stop entity = new Stop();
            entity.StopID = reader.GetString(0);
            entity.StopName = reader.GetString(1);
            entity.Latitude = reader.GetDouble(2);
            entity.Longitude = reader.GetDouble(3);
            return entity;
        }
    }
}

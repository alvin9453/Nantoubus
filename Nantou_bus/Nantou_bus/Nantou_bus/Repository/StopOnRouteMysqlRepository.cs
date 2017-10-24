using System.Collections.Generic;

using MySql.Data.MySqlClient;
using Nantou_bus.Model;

namespace Nantou_bus.Repository
{
    public class StopOnRouteMysqlRepository : AbstractMysqlRepository<StopOnRoute>
    {
        public static StopOnRouteMysqlRepository Instance
        {
            get
            {
                if (_Instance == null) { _Instance = new StopOnRouteMysqlRepository(); }
                return _Instance;
            }
        }

        private static StopOnRouteMysqlRepository _Instance;

        private const string SQL_LIST = "SELECT RouteUID, SubRouteUID, Direction, StopID FROM `bus_stops_on_route` WHERE StopID = {0}";

        public IEnumerable<StopOnRoute> Retrieve(string stopID)
        {
            IEnumerable<StopOnRoute> results = Query( string.Format(SQL_LIST,stopID) );
            return results;
        }

        protected override StopOnRoute ToEntity(MySqlDataReader reader)
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

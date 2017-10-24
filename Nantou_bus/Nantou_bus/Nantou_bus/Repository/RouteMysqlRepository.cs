using System.Collections.Generic;
using System.Linq;

using MySql.Data.MySqlClient;
using Nantou_bus.Model;

namespace Nantou_bus.Repository
{
    public class RouteMysqlRepository : AbstractMysqlRepository<Route>
    {
        public static RouteMysqlRepository Instance
        {
            get
            {
                if (_Instance == null) { _Instance = new RouteMysqlRepository(); }
                return _Instance;
            }
        }

        private static RouteMysqlRepository _Instance;

        private const string SQL_GET = "SELECT SubRouteUID, Direction, Headsign, SubRouteName, SubRouteName_en, RouteUID, RouteID FROM bus_routes WHERE SubRouteUID='{0}'";

        private const string SQL_GET_BACKROUTE = "SELECT SubRouteUID, Direction, Headsign, SubRouteName, SubRouteName_en, RouteUID, RouteID FROM bus_routes WHERE SubRouteName='{0}'";

        public Route Get(string routeUID)
        {
            string sql = string.Format(SQL_GET, routeUID);
            IEnumerable<Route> results = Query(sql);
            int count = Enumerable.Count<Route>(results);
            if (count == 0) { return null; }
            return results.First();
        }

        public IEnumerable<Route> GetBackRoute(string subRouteName)
        {
            string sql = string.Format(SQL_GET_BACKROUTE, subRouteName);
            IEnumerable<Route> results = Query(sql);
            return results;
        }

        protected override Route ToEntity(MySqlDataReader reader)
        {
            Route entity = new Route();
            Name name = new Name();
            entity.UID = reader.GetString(0);
            entity.Direction = reader.GetBoolean(1);
            entity.HeadSign = reader.GetString(2);
            name.Zh_tw = reader.GetString(3);
            name.En = reader.GetString(4);
            entity.Name = name;
            entity.ParentUID = reader.GetString(5);
            entity.Number = reader.GetString(6);
            return entity;
        }
    }
}

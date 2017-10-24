using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Linq;

using Nantou_bus.Model;

namespace Nantou_bus.Repository
{
    public class RouteSQLiteRepository : AbstractSQLiteRepository<Route>
    {
        public static RouteSQLiteRepository Instance
        {
            get
            {
                if (_Instance == null) { _Instance = new RouteSQLiteRepository(); }
                return _Instance;
            }
        }

        private static RouteSQLiteRepository _Instance;

        private const string SQL_SELECT_CLAUSE = "SELECT SubRouteUID, Direction, Headsign, SubRouteName, SubRouteName_en, RouteUID, RouteID FROM bus_routes";

        private const string SQL_CRITERIA_ROUTE_ID = SQL_SELECT_CLAUSE + " WHERE RouteID LIKE '%{0}%' ORDER BY SubRouteUID";

        private const string SQL_CRITERIA_ROUTE_UID = SQL_SELECT_CLAUSE + " WHERE RouteUID='{0}' ORDER BY SubRouteUID";

        private const string SQL_CRITERIA_SUBROUTE_ID = SQL_SELECT_CLAUSE + " WHERE SubRouteUID='{0}'";

        private const string SQL_CRITERIA_SUBROUTE_NAME = SQL_SELECT_CLAUSE + " WHERE SubRouteName='{0}'";

        private const string SQL_CRITERIA_SUBROUTE_UID = SQL_SELECT_CLAUSE + " WHERE SubRouteUID='{0}' ORDER BY SubRouteUID";

        public Route Get(string subrouteUid)
        {
            string sql = string.Format(SQL_CRITERIA_SUBROUTE_ID, subrouteUid);
            IEnumerable<Route> results = Query(sql);
            int count = Enumerable.Count<Route>(results);
            if (count == 0) { return null; }
            return results.First();
        }

        public IEnumerable<Route> GetBackRoute(string subRouteName)
        {
            string sql = string.Format(SQL_CRITERIA_SUBROUTE_NAME, subRouteName);
            IEnumerable<Route> results = Query(sql);
            return results;
        }

        public IEnumerable<Route> GetRoute(IEnumerable<string> routeuids)
        {
            IEnumerable<Route> results = new List<Route>();
            foreach (string routeuid in routeuids) { results = results.Concat(GetRoute(routeuid)); }
            return results;
        }

        public IEnumerable<Route> GetRoute(string routeuid)
        {
            string sql = string.Format(SQL_CRITERIA_ROUTE_UID, routeuid);
            IEnumerable<Route> results = Query(sql);
            return results;
        }

        public IEnumerable<Route> GetSubroute(IEnumerable<string> subrouteuids)
        {
            IEnumerable<Route> results = new List<Route>();
            foreach (string subrouteuid in subrouteuids) { results = results.Concat(GetSubtoute(subrouteuid)); }
            return results;
        }

        public IEnumerable<Route> GetSubtoute(string subrouteuid)
        {
            string sql = string.Format(SQL_CRITERIA_SUBROUTE_UID, subrouteuid);
            IEnumerable<Route> results = Query(sql);
            return results;
        }

        public IEnumerable<Route> GetSubroute(string subrouteID)
        {
            string sql = string.Format(SQL_CRITERIA_ROUTE_ID, subrouteID);
            IEnumerable<Route> results = Query(sql);
            int count = Enumerable.Count<Route>(results);
            if (count == 0) { return null; }
            return results;
        }

        protected override Route ToEntity(SqliteDataReader reader)
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

using System.Collections.Generic;

using MySql.Data.MySqlClient;
using Nantou_bus.Model;

namespace Nantou_bus.Repository
{
    public class RouteStopPredictionMysqlRepository : AbstractMysqlRepository<RouteStopPrediction>
    {
        public static RouteStopPredictionMysqlRepository Instance
        {
            get
            {
                if (_Instance == null) { _Instance = new RouteStopPredictionMysqlRepository(); }
                return _Instance;
            }
        }

        private static RouteStopPredictionMysqlRepository _Instance;

        private const string SQL_LIST = "SELECT RouteID, RouteName, bus_time FROM `timetable`";

        public IEnumerable<RouteStopPrediction> List()
        {
            IEnumerable<RouteStopPrediction> results = Query(SQL_LIST);
            return results;
        }

        protected override RouteStopPrediction ToEntity(MySqlDataReader reader)
        {
            RouteStopPrediction entity = new RouteStopPrediction();
            entity.RouteID = reader.GetString(0);
            entity.RouteName = reader.GetString(1);
            entity.MinutesToArrival = reader.GetString(2);
            return entity;
        }
    }
}

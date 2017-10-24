using MySql.Data.MySqlClient;
using Nantou_bus.Model;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Nantou_bus.Repository
{
    public class StopMysqlRepository : AbstractMysqlRepository<Stop>
    {
        public static StopMysqlRepository Instance
        {
            get
            {
                if (_Instance == null) { _Instance = new StopMysqlRepository(); }
                return _Instance;
            }
        }

        private static StopMysqlRepository _Instance;

        private const string SQL_GET = "SELECT StopID, StopName, LAT, LON FROM `bus_stops` GROUP BY `StopName`";
 
        public IEnumerable<Stop> List()
        {
            IEnumerable<Stop> results = Query(SQL_GET);
            return results;
        }

        protected override Stop ToEntity(MySqlDataReader reader)
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

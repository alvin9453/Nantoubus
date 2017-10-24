using System.Collections.Generic;
using System.Net;

using Newtonsoft.Json;

namespace Nantou_bus.Model.TransportData
{
    public class BusRoute
    {
        public string RouteUID { get; set; }
        public string RouteID { get; set; }
        public Name RouteName { get; set; }
        public List<string> OperatorIDs { get; set; }
        public string AuthorityID { get; set; }
        public string DepartureStopNameZh { get; set; }
        public string DepartureStopNameEn { get; set; }
        public string DestinationStopNameZh { get; set; }
        public string DestinationStopNameEn { get; set; }
        public List<BusSubRoute> SubRoutes { get; set; }
        public string UpdateTime { get; set; }

        public class BusSubRoute
        {
            public string SubRouteUID { get; set; }
            public string SubRouteID { get; set; }
            public Name SubRouteName { get; set; }
            public string HeadSign { get; set; }
            public string Direction { get; set; }
        }

        public static List<BusRoute> RetrieveFromJson(string url)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string jsonString = client.DownloadString(url);
                    List<BusRoute> entities = JsonConvert.DeserializeObject<List<BusRoute>>(jsonString);
                    return entities;
                }
            }
            catch (WebException ex) { return new List<BusRoute>(); }
        }
    }
}

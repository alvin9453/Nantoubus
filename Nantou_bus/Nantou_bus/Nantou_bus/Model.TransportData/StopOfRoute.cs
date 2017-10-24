using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Nantou_bus.Model.TransportData
{
    public class StopOfRoute
    {
        public string RouteUID { get; set; }
        public string RouteID { get; set; }
        public Name RouteName { get; set; }
        public string OperatorID { get; set; }
        public bool KeyPattern { get; set; }
        public string SubRouteUID { get; set; }
        public string SubRouteID { get; set; }
        public Name SubRouteName { get; set; }
        public int Direction { get; set; }
        public IList<Stop> Stops { get; set; }
        public string UpdateTime { get; set; }

        public int GetStopIndex(IEnumerable<string> stopUids)
        {
            for (int i = 0; i < Stops.Count; i++)
            {
                if (stopUids.Contains(Stops[i].StopUID)) { return i; }
            }
            return -1;
        }

        public static List<StopOfRoute> RetrieveFromJson(string url)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string jsonString = client.DownloadString(url);
                    List<StopOfRoute> entities = JsonConvert.DeserializeObject<List<StopOfRoute>>(jsonString);
                    return entities;
                }
            }
            catch (WebException ex) { return new List<StopOfRoute>(); }
        }

        public class Position
        {
            public double PositionLat { get; set; }
            public double PositionLon { get; set; }
        }

        public class Stop
        {
            public string StopUID { get; set; }
            public string StopID { get; set; }
            public Name StopName { get; set; }
            public int StopBoarding { get; set; }
            public int StopSequence { get; set; }
            public Position StopPosition { get; set; }
        }
    }
}

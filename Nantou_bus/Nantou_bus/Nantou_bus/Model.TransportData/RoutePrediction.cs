using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace Nantou_bus.Model.TransportData
{
    public class RoutePrediction
    {
        public string PlateNumb { get; set; }
        public string StopUID { get; set; }
        public string StopID { get; set; }
        public Name StopName { get; set; }
        public string RouteUID { get; set; }
        public string RouteID { get; set; }
        public Name RouteName { get; set; }
        public string SubRouteUID { get; set; }
        public string SubRouteID { get; set; }
        public Name SubRouteName { get; set; }
        public int Direction { get; set; }
        public int StopCountDown { get; set; }
        public string CurrentStop { get; set; }
        public string DestinationStop { get; set; }
        public int StopStatus { get; set; }
        public int MessageType { get; set; }
        public bool IsLastBus { get; set; }
        public string TransTime { get; set; }
        public string SrcRecTime { get; set; }
        public string UpdateTime { get; set; }
        public int StopSequence { get; set; }
        public int EstimateTime { get; set; }

        public static List<RoutePrediction> RetrieveFromJson(string url)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string jsonString = client.DownloadString(url);
                    List<RoutePrediction> entities = JsonConvert.DeserializeObject<List<RoutePrediction>>(jsonString);
                    return entities;
                }
            }
            catch (WebException ex) { return new List<RoutePrediction>(); }
        }
    }
}

namespace Nantou_bus.Model
{
    public class StopOnRoute
    {
        public string RouteUID { get; set; }

        public string SubRouteUID { get; set; }

        public bool Direction { get; set; }

        public string StopID { get; set; }
    }
}

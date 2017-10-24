using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

using Nantou_bus.Model.TransportData;
using Nantou_bus.UI.View;

namespace Nantou_bus
{
    public class RoutePage : ContentPage
    {
        private const string URL_STOP_OF_ROUTE = "http://ptx.transportdata.tw/MOTC/v2/Bus/StopOfRoute/InterCity/{0}?$format=JSON";
        private const string URL_ESTIMATED_ARRIVAL = "http://ptx.transportdata.tw/MOTC/v2/Bus/EstimatedTimeOfArrival/InterCity/{0}?$orderby=StopSequence&$filter=SubRouteUID%20eq%20%27{1}%27%20&$format=JSON";

        private static double LABEL_FONT_SIZE = Device.GetNamedSize(NamedSize.Large, typeof(Label));

        public RoutePage(String RouteID, String SubRouteUID)
        {
            PopulateRoute(RouteID, SubRouteUID);
        }

        private void PopulateRoute(String routeID, String subRouteUID)
        {
            IEnumerable<StopOfRoute> stopsOfSingleRoute = StopOfRoute.RetrieveFromJson(string.Format(URL_STOP_OF_ROUTE, routeID))
                .Where(entity => string.Equals(entity.SubRouteUID, subRouteUID));
            if (stopsOfSingleRoute.Count() == 0) { return; }

            IEnumerable<RoutePrediction> predictions = RoutePrediction.RetrieveFromJson(string.Format(URL_ESTIMATED_ARRIVAL, routeID, subRouteUID));
            StopArrivalPrediction view = new StopArrivalPrediction(stopsOfSingleRoute.First(), predictions);
            Content = new ScrollView { Content = view };
        }
    }
}
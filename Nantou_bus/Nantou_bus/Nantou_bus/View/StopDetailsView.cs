using System;
using System.Collections.Generic;
using System.Linq;

using Nantou_bus.Model;
using Nantou_bus.Model.TransportData;
using Nantou_bus.Repository;
using Xamarin.Forms;
using Nantou_bus.UI.View;
using Nantou_bus.View;

namespace Nantou_bus
{
    public class StopDetailsView : ContentPage
    {
        private bool CheckArriveRoute = false;

        private Layout<Xamarin.Forms.View> PageLayout;

        private const string GOOGLE_MAP_KEY = "AIzaSyDUUf5gsUvFYC-s7D6iCAUvn19Yh09rdMM";

        public StopDetailsView() : base()
        {
            PageLayout = new StackLayout { Margin = 10, Spacing = 10 };
        }

        public StopDetailsView(StopDetailsViewConfig config) : this()
        {
            Title = config.stopName;
            ToolbarItem toolbarItem = new ToolbarItem { Text = "顯示行經路線" };
            toolbarItem.Clicked += (object sender, EventArgs e) => { Navigation.PushAsync(new StopPassingRouteView(config.stopName, config.stopID)); };
            ToolbarItems.Add(toolbarItem);
            PageLayout.Children.Add(new GoogleStreetView(config.latitude, config.longitude));
            PopulateArriveRoute(config.stopID, config.stopName);
            Content = new ScrollView { Content = PageLayout, Margin = 10 };
        }

        public class StopDetailsViewConfig
        {
            public double latitude;
            public double longitude;
            public string stopID;
            public string stopName;

            public StopDetailsViewConfig(double latitude, double longitude, String stopID, String stopName)
            {
                this.latitude = latitude;
                this.longitude = longitude;
                this.stopID = stopID;
                this.stopName = stopName;
            }
        }

        private Button CreateRouteArriveButton(RoutePrediction arriveRoute, IEnumerable<string> skipUID,List<string>processedRouteID)
        {
            string routeUID = arriveRoute.SubRouteUID;
            if (skipUID.Contains(routeUID)) { return null; }
            routeUID = routeUID.Replace(" ", "");
            Route route = RouteSQLiteRepository.Instance.Get(routeUID);
            if (!processedRouteID.Contains(route.Number))
            {
                processedRouteID.Add(route.Number);
                PageLayout.Children.Add(new Label { Text = route.Number, FontSize = 20, TextColor = Color.Red });
            }
            return CreateRouteArriveButton(arriveRoute, route, routeUID);
        }

        private Button CreateRouteArriveButton(RoutePrediction arriveRoute, Route route, string routeUID = null)
        {
            Button button = new Button();
            if (routeUID == null) { routeUID = arriveRoute.SubRouteUID.Replace(" ", ""); }
            RouteView.RouteViewConfig Config = new RouteView.RouteViewConfig(arriveRoute.RouteID, routeUID, arriveRoute.SubRouteName.En, route.HeadSign, FindBackSubRouteUID(routeUID, route));
            arriveRoute.EstimateTime = arriveRoute.EstimateTime / 60;
            button.Text = string.Format("{0}\t {1}分鐘", new string[] { route.HeadSign, arriveRoute.EstimateTime.ToString() });
            button.Clicked += (object sender, EventArgs e) => { Navigation.PushAsync(new RouteView(Config)); };
            return button;
        }

        private String FindBackSubRouteUID(String routeUID, Route route)
        {
            string backSubRouteUID = "";
            string subRouteName = route.Name.Zh_tw;
            IEnumerable<Route> backRoutes = RouteSQLiteRepository.Instance.GetBackRoute(subRouteName);
            if (Enumerable.Count<Route>(backRoutes) == 1) { return null; };
            foreach (Route backRoute in backRoutes)
            {
                if (backRoute.UID == routeUID) { continue; }
                else{backSubRouteUID = backRoute.UID;}
            }
            return backSubRouteUID;
        }

        private void PopulateArriveRoute(String stopID, String stopName)
        {
            PageLayout.Children.Add(new Label { Text = "即將到達路線" });
            IEnumerable<StopOnRoute> StopsOnSingleRoute = StopOnRouteSQLiteRepository.Instance.Retrieve(stopID);

            List<string> processedRouteUID = new List<string>();
            List<string> processedRouteID = new List<string>();
            foreach (StopOnRoute StopOnSingleRoute in StopsOnSingleRoute)
            {
                IEnumerable<RoutePrediction> arriveRoutes = RoutePrediction.RetrieveFromJson(string.Format("http://ptx.transportdata.tw/MOTC/v2/Bus/EstimatedTimeOfArrival/InterCity?$filter=SubRouteUID%20eq%20%27{0}%27%20and%20StopName/Zh_tw%20eq%20%27{1}%27&$top=30&$format=JSON", StopOnSingleRoute.SubRouteUID, stopName));
                foreach (RoutePrediction arriveRoute in arriveRoutes)
                {
                    if (arriveRoute.PlateNumb != "-1")
                    {
                        Button routeDetails = CreateRouteArriveButton(arriveRoute, processedRouteUID, processedRouteID);
                        CheckArriveRoute = true;
                        if (routeDetails == null) { continue; }
                        processedRouteUID.Add(arriveRoute.SubRouteUID);
                        PageLayout.Children.Add(routeDetails);
                    }
                    else continue;
                }
            }
            if (!CheckArriveRoute) { PageLayout.Children.Add(new Label { Text = "本站無即將到達路線", FontSize = 20, TextColor = Color.Red, HorizontalOptions = LayoutOptions.CenterAndExpand }); }
        }
    }
}
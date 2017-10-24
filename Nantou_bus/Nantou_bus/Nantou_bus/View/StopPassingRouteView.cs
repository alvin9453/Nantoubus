using Nantou_bus.Model;
using Nantou_bus.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;

namespace Nantou_bus.View
{
    public class StopPassingRouteView : ContentPage
	{
        private Layout<Xamarin.Forms.View> PageLayout;

        public StopPassingRouteView() : base()
        {
            PageLayout = new StackLayout { Spacing = 10, Margin = 10};
        }

        public StopPassingRouteView (string stopName,string stopID) : this()
		{
            Title = stopName;
            PopulatePassingByRoutes(stopID);
            Content = new ScrollView { Content = PageLayout };
        }

        private Button CreateRouteDetailsButton(StopOnRoute stopOnRoute, IEnumerable<string> skipUIDs,List<string> processedRouteIDs)
        {
            string routeUID = stopOnRoute.SubRouteUID;
            if (skipUIDs.Contains(routeUID)) { return null; }
            Route route = RouteSQLiteRepository.Instance.Get(routeUID);
            if (!processedRouteIDs.Contains(route.Number))
            {
                processedRouteIDs.Add(route.Number);
                PageLayout.Children.Add(new Label { Text = route.Number,FontSize = 20,TextColor = Color.Red });
            };
            return CreateRouteDetailsButton(stopOnRoute, route, FindBackSubRouteUID(routeUID, route), routeUID);
        }

        private Button CreateRouteDetailsButton(StopOnRoute stopOnRoute, Route route, string backSubRouteUID, string routeUID = null)
        {
            Button button = new Button();
            if (routeUID == null) { routeUID = stopOnRoute.SubRouteUID.Replace(" ", ""); }
            string routeID = route.ParentUID.Substring(10);
            RouteView.RouteViewConfig Config = new RouteView.RouteViewConfig(routeID, routeUID, route.Name.En, route.HeadSign, backSubRouteUID);
            button.Text = string.Format("{0}", new string[] {route.HeadSign });
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
                else
                {
                    backSubRouteUID = backRoute.UID;
                }
            }
            return backSubRouteUID;
        }

        private void PopulatePassingByRoutes(string stopID)
        {
            IEnumerable<StopOnRoute> StopsOnSingleRoute = StopOnRouteSQLiteRepository.Instance.Retrieve(stopID);
            List<string> processedRouteUIDs = new List<string>();
            List<string> processedRouteIDs = new List<string>();
            foreach (StopOnRoute StopOnSingleRoute in StopsOnSingleRoute)
            {
                Button routeDetails = CreateRouteDetailsButton(StopOnSingleRoute, processedRouteUIDs,processedRouteIDs);
                if (routeDetails == null) { continue; }
                processedRouteUIDs.Add(StopOnSingleRoute.SubRouteUID);
                PageLayout.Children.Add(routeDetails);
            }
        }
    }
}
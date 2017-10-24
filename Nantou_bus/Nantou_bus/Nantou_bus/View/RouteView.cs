using System;

using Xamarin.Forms;

namespace Nantou_bus
{
    public class RouteView : TabbedPage
    {
        public RouteView(RouteViewConfig config)
        {
            Title = string.Format("{0}\t{1}", config.routeID, config.headSign);
            ToolbarItem tbi = new ToolbarItem();
            tbi.Text = "聯繫客運";
            tbi.Clicked += (object sender, EventArgs e) => { this.Navigation.PushAsync(new ContactPage(config.routeID)); };
            this.ToolbarItems.Add(tbi);
            Children.Add(new RoutePage(config.routeID, config.subRouteUID) { Title = "去程" });
            if (config.backSubRouteUID != null) { Children.Add(new RoutePage(config.routeID, config.backSubRouteUID) { Title = "回程" }); };
            Children.Add(new TimeTablePage(config.routeID, config.subRouteName) { Title = "時刻表" });
        }

        public class RouteViewConfig
        {
            public string routeID;
            public string subRouteUID;
            public string subRouteName;
            public string headSign;
            public string backSubRouteUID;

            public RouteViewConfig(String routeID, String subRouteUID, String subRouteName, String headSign, String backSubRouteUID)
            {
                this.routeID = routeID;
                this.subRouteUID = subRouteUID;
                this.subRouteName = subRouteName;
                this.headSign = headSign;
                this.backSubRouteUID = backSubRouteUID;
            }
        }


    }
}
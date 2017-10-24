using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Nantou_bus.Model.TransportData;
using Nantou_bus.Repository;
using Nantou_bus.Model;

namespace Nantou_bus
{
	public class NumberSearch : ContentPage
	{
        private Layout<Xamarin.Forms.View> MainStack;
        Label NumberLabel;
        ScrollView SubrouteButtonView = new ScrollView { };
        string InputNumber = "";
        List<BusRoute.BusSubRoute> SubRoutes = null;
        public NumberSearch()
        {
            Title = "路線明細";
            MainStack = new StackLayout
            {
                Margin = 10,
            };
            PopulateSearch();
            PopulateRouteName();
            MainStack.Children.Add(SubrouteButtonView);
            Content = MainStack;
        }

        private void PopulateSearch()
        {
            StackLayout SearchStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            Entry NumberEntry = new Entry
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Entry)),
                Placeholder = "  請輸入路線編號  ",
            };
            Button SearchButton = new Button
            {
                HorizontalOptions = LayoutOptions.End,
                Text = "搜尋",
            };
            SearchButton.Clicked += ButtonSearchClicked;
            void ButtonSearchClicked(object sender, EventArgs e)
            {
                InputNumber = NumberEntry.Text;
                NumberLabel.Text = "搜尋路線編號 :" + InputNumber;

                PopulateSubRoute();
            }
            SearchStack.Children.Add(NumberEntry);
            SearchStack.Children.Add(SearchButton);
            MainStack.Children.Add(SearchStack);
        }

        private void PopulateRouteName()
        {
            NumberLabel = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                Text = "",
            };
            MainStack.Children.Add(NumberLabel);
        }

        private void PopulateSubRoute()
        {
            StackLayout SubrouteButtonStack = new StackLayout { };
            IEnumerable<Route> subroutes;
            List<Button> subrouteButtons = new List<Button>();
            if(int.TryParse(InputNumber, out int ParseResult))
            {
                subroutes = RouteSQLiteRepository.Instance.GetSubroute(InputNumber);
                if (subroutes == null) { SubrouteButtonStack.Children.Add(new Label { Text = "查無路線", FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)) }); }
                else
                {
                    foreach (Route subRoute in subroutes)
                    {
                        String routeID = subRoute.Number;   // != InputNumber
                        Button subrouteButton = CreateSubrouteButton(routeID, subRoute);
                        subrouteButtons.Add(subrouteButton);
                        SubrouteButtonStack.Children.Add(subrouteButton);
                    }
                }
            }
            else
            {
                SubrouteButtonStack.Children.Add(new Label { Text = "請輸入路線編號", FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)) });
            }
            SubrouteButtonView.Content = SubrouteButtonStack;
        }

        private Button CreateSubrouteButton(String RouteID, Route SubRoute)
        {
            Button button = new Button();
            String SubRouteUID = SubRoute.UID.ToString();   // THB-VO11-1667001
            String SubRouteName = SubRoute.Name.En.ToString();   // 667001
            String SubBouteHead = SubRoute.HeadSign.ToString();
            String BackSubRouteUID = FindSubRouteBack(SubRouteName, SubRouteUID);
            RouteView.RouteViewConfig Config = new RouteView.RouteViewConfig(RouteID, SubRouteUID, SubRouteName, SubBouteHead, BackSubRouteUID);
            button.Text = RouteID + "\t" + SubBouteHead;
            button.Clicked += (object sender, EventArgs e) => { this.Navigation.PushAsync(new RouteView(Config)); };
            return button;
        }

        private String FindSubRouteBack(string SubRouteName, string SubRouteUID)
        {
            string backSubRouteUID = "";
            IEnumerable<Route> backRoutes = RouteSQLiteRepository.Instance.GetBackRoute(SubRouteName);
            if (Enumerable.Count<Route>(backRoutes) == 1) { return null; };
            foreach (Route backRoute in backRoutes)
            {
                if (backRoute.UID == SubRouteUID) { continue; }
                else
                {
                    backSubRouteUID = backRoute.UID;
                }
            }
            return backSubRouteUID;
        }
    }
}
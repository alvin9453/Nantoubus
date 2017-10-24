using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Nantou_bus.Model;
using Nantou_bus.Repository;

namespace Nantou_bus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchHistory : ContentPage
    {
        private String History1;

        private String History2;

        private String History3;

        private ScrollView MainSrollView = new ScrollView { };

        public SearchHistory()
        {
            InitializeComponent();
            mainstack.Children.Add(MainSrollView);
        }

        private Layout CreateLayoutFromRoutes(IEnumerable<Route> routes)
        {
            StackLayout layout = new StackLayout();
            foreach (Route route in routes) { layout.Children.Add(CreateRouteButton(route)); }
            return layout;
        }

        private Button CreateRouteButton(Route route)
        {
            Button button = new Button();
            string number = route.Number;
            string headsign = route.HeadSign;
            button.Text = number + " " + headsign;
            String BackSubRouteUID = FindSubRouteBack(route.Name.En, route.UID);
            RouteView.RouteViewConfig config = new RouteView.RouteViewConfig(number, route.UID, route.Name.En, headsign, BackSubRouteUID);
            button.Clicked += (object sender2, EventArgs e2) => { Navigation.PushAsync(new RouteView(config)); };
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

        private void Exchange_Clicked(object sender, EventArgs e)
        {
            String temp;
            temp = StartRouteSearchBar.Text;
            StartRouteSearchBar.Text = ReachRouteSearchBar.Text;
            ReachRouteSearchBar.Text = temp;
        }

        private String GetLastCreateName<T>() where T : AbstractRecord, new()
        {
            IEnumerable<T> existed = MapPage.localDatabase.List<T>();
            if (existed.Count() > 0) { return existed.Last().Name; }
            if (typeof(MyRecord).Equals(typeof(T))) { return "尚未設定住家喜好站牌"; }
            if (typeof(OfficeRecord).Equals(typeof(T))) { return "尚未設定公司喜好站牌"; }
            if (typeof(SchoolRecord).Equals(typeof(T))) { return "尚未設定學校喜好站牌"; }
            return null;
        }

        private void LoadHistoryRecords()
        {
            List<HistoryRecord> allRecords = MapPage.localDatabase.List<HistoryRecord>();
            int count = allRecords.Count();
            if (count > 0) { History1 = allRecords[count - 1].Name; }
            History1 = (count > 0) ? allRecords[count - 1].Name : "暫無查詢記錄1";
            History2 = (count > 1) ? allRecords[count - 2].Name : "暫無查詢記錄2";
            History3 = (count > 2) ? allRecords[count - 3].Name : "暫無查詢記錄3";
        }

        private void LoadPreferedRecords()
        {
            History1 = GetLastCreateName<MyRecord>();
            History2 = GetLastCreateName<OfficeRecord>();
            History3 = GetLastCreateName<SchoolRecord>();
        }

        private async void SearchF_Clicked(object sender, EventArgs e)
        {
            LoadPreferedRecords();
            string[] candidates = new string[] { History1, History2, History3 };
            string action = await DisplayActionSheet("預存位置-起站", "Cancel", null, candidates);
            if (candidates.Contains(action)) { StartRouteSearchBar.Text = action; }
        }

        private async void SearchF2_Clicked(object sender, EventArgs e)
        {
            LoadPreferedRecords();
            string[] candidates = new string[] { History1, History2, History3 };
            string action = await DisplayActionSheet("預存位置-訖站", "Cancel", null, candidates);
            if (candidates.Contains(action)) { ReachRouteSearchBar.Text = action; }
        }

        private async void SearchH_Clicked(object sender, EventArgs e)
        {
            LoadHistoryRecords();
            string[] candidates = new string[] { History1, History2, History3 };
            string action = await DisplayActionSheet("查詢記錄-起站", "Cancel", null, candidates);
            if (candidates.Contains(action)) { StartRouteSearchBar.Text = action; }
        }

        private async void SearchH2_Clicked(object sender, EventArgs e)
        {
            LoadHistoryRecords();
            string[] candidates = new string[] { History1, History2, History3 };
            string action = await DisplayActionSheet("查詢記錄-訖站", "Cancel", null, candidates);
            if (candidates.Contains(action)) { ReachRouteSearchBar.Text = action; }
        }

        private void SaveSearchingStops(string fromStopName, string toStopName)
        {
            HistoryRecord from = new HistoryRecord { Name = fromStopName };
            HistoryRecord to = new HistoryRecord { Name = toStopName };
            MapPage.localDatabase.Create<HistoryRecord>(from);
            MapPage.localDatabase.Create<HistoryRecord>(to);
        }

        private void SearchNOW_Clicked(object sender, EventArgs e)
        {
            SaveSearchingStops(StartRouteSearchBar.Text, ReachRouteSearchBar.Text);
            List<string> startStopUids = StopSQLiteRepository.Instance
                .Get(StartRouteSearchBar.Text).Select(entity => entity.StopID).ToList();
            List<string> endStopUids = StopSQLiteRepository.Instance
                .Get(ReachRouteSearchBar.Text).Select(entity => entity.StopID).ToList();
            if (startStopUids.Count() <= 0 || endStopUids.Count() <= 0)
            {
                DisplayAlert("提醒", "查無此站牌", "重新輸入");
                StartRouteSearchBar.Text = "";
                ReachRouteSearchBar.Text = "";
                return;
            }

            StopOnRouteSQLiteRepository stopOnRouteRepository = StopOnRouteSQLiteRepository.Instance;
            IEnumerable<string> subrouteUidsPassStartStops = stopOnRouteRepository.Retrieve(startStopUids).Select(stopOnRoute => stopOnRoute.SubRouteUID).Distinct();
            IEnumerable<string> subrouteUidsPassEndStops = stopOnRouteRepository.Retrieve(endStopUids).Select(stopOnRoute => stopOnRoute.SubRouteUID).Distinct();
            IEnumerable<string> subrouteUids = subrouteUidsPassStartStops.Intersect(subrouteUidsPassEndStops);
            IEnumerable<Route> routes = RouteSQLiteRepository.Instance.GetSubroute(subrouteUids);
            MainSrollView.Content = CreateLayoutFromRoutes(routes);
        }
    }
}
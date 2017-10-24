using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

using Nantou_bus.Model;
using Nantou_bus.Repository;
using Nantou_bus.UI.Map;

namespace Nantou_bus
{
    public class MapPage : ContentPage
    {
        public static LocalDatabase localDatabase;

        private Layout<Xamarin.Forms.View> PageLayout;

        public MapPage() : base()
        {
            Title = "地圖查找";
            PageLayout = new StackLayout { Spacing = 10 };
            localDatabase = new LocalDatabase();
            PageLayout.Children.Add(CreateMapView());
            
            Content = PageLayout;
        }

        private Xamarin.Forms.View CreateMapView()
        {
            Position startPosition = new Position(23.949644, 120.934703);
            CustomMap map = new CustomMap();
            IEnumerable<Stop> stops = StopSQLiteRepository.Instance.List();
            IEnumerable<CustomPin> stopsPins = stops.Select(stop => CreateStopPin(stop, map));
            foreach (CustomPin pin in stopsPins)
            {
                map.CustomPinsTest.Add(pin);
                map.Pins.Add(pin.Pin);
                map.CustomPins.Add(pin);
            };
            map.MoveToRegion(MapSpan.FromCenterAndRadius(startPosition, Distance.FromMiles(0.5)));
            return map;
        }

        private CustomPin CreateStopPin(Stop stop, CustomMap customMap)
        {
            CustomPin pin = new CustomPin
            {
                Position = new Position(stop.Latitude, stop.Longitude),
                Label = stop.StopName,
                Pin = new Pin
                {
                    Position = new Position(stop.Latitude, stop.Longitude),
                    Label = stop.StopName
                },
                Id = stop.StopName
            };
            StopDetailsView.StopDetailsViewConfig Config = new StopDetailsView.StopDetailsViewConfig(stop.Latitude, stop.Longitude, stop.StopID, stop.StopName);
            pin.InfoTapped += (sender, e) =>
            {
                Page stopDetails = new StopDetailsView(Config);
                Navigation.PushAsync(stopDetails);
            };
            pin.CalloutAccessoryTapped += (sender, e) =>
             {
                 Page stopDetails = new StopDetailsView(Config);
                 Navigation.PushAsync(stopDetails);
             };
            return pin;
        }
    }
}
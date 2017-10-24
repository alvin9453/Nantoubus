using System;
using CoreLocation;
using Nantou_bus.iOS;
using Xamarin.Forms;
using Nantou_bus.Interface.Location;

[assembly: Dependency(typeof(GetMyLocation))]
namespace Nantou_bus.iOS
{
    public class LocationEventArgs : EventArgs, ILocationEventArgs
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
    public class GetMyLocation : ILocation
    {
        public event EventHandler<ILocationEventArgs> locationObtained;

        public void ObtainMyLocation()
        {
            CLLocationManager lm = new CLLocationManager();
            lm.DesiredAccuracy = CLLocation.AccuracyBest;
            lm.DistanceFilter = CLLocationDistance.FilterNone;

            lm.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>
            {
                CLLocation[] locations = e.Locations;
                string strLocation = locations[locations.Length - 1].Coordinate.Latitude.ToString();
                strLocation = strLocation + "," + locations[locations.Length - 1].Coordinate.Longitude.ToString();
                LocationEventArgs args = new LocationEventArgs();
                args.latitude = locations[locations.Length - 1].Coordinate.Latitude;
                args.longitude = locations[locations.Length - 1].Coordinate.Longitude;
                locationObtained(this, args);
            };
            lm.AuthorizationChanged += (object sender, CLAuthorizationChangedEventArgs e) =>
            {
                if (e.Status == CLAuthorizationStatus.AuthorizedWhenInUse)
                {
                    lm.StartUpdatingLocation();
                }
            };
            lm.RequestWhenInUseAuthorization();

        }
    }
}

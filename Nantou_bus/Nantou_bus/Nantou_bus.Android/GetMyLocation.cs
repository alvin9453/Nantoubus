using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Nantou_bus.Droid;
using Nantou_bus.Interface.Location;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(GetMyLocation))]
namespace Nantou_bus.Droid
{
    public class LocationEventArgs : EventArgs, ILocationEventArgs
    {
        public double latitude { get; set; }
        public double longitude { get; set; }

    }

    public class GetMyLocation : Java.Lang.Object, ILocation, ILocationListener
    {
        public event EventHandler<ILocationEventArgs> locationObtained;

        public void ObtainMyLocation()
        {
            LocationManager locationManager = (LocationManager)Forms.Context.GetSystemService(Context.LocationService);
            locationManager.RequestLocationUpdates(LocationManager.GpsProvider, 0, 0, this);
        }

        public void OnLocationChanged(Location location)
        {
            if (location != null)
            {
                LocationEventArgs args = new LocationEventArgs();
                args.latitude = location.Latitude;
                args.longitude = location.Longitude;
                locationObtained(this, args);
            }
            else
            {
                LocationEventArgs args = new LocationEventArgs();
                args.latitude = 23.1234;
                args.longitude = 120.1234;
                locationObtained(this, args);
            }
        }

        public void OnProviderDisabled(string provider)
        {
        }

        public void OnProviderEnabled(string provider)
        {
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
        }
    }
}

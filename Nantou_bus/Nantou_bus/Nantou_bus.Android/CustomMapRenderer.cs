using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Widget;
using Nantou_bus.Droid;
using Nantou_bus.UI.Map;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace Nantou_bus.Droid
{
    public class CustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter, IOnMapReadyCallback
    {
        private bool IsPinMarkersUpdated;

        private IList<CustomPin> Pins;

        public Android.Views.View GetInfoContents(Marker marker)
        {
            Android.Views.LayoutInflater inflater = Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as Android.Views.LayoutInflater;
            if (inflater == null) { return null; }

            CustomPin customPin = GetCustomPin(marker);
            if (customPin == null) { throw new Exception("Custom pin not found"); }

            Android.Views.View infoContents = inflater.Inflate(Resource.Layout.MapInfoWindow, null);
            TextView infoTitle = infoContents.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
            TextView infoSubtitle = infoContents.FindViewById<TextView>(Resource.Id.InfoWindowSubtitle);
            if (infoTitle != null) { infoTitle.Text = marker.Title; }
            if (infoSubtitle != null) { infoSubtitle.Text = marker.Snippet; }

            return infoContents;
        }

        public Android.Views.View GetInfoWindow(Marker marker) { return null; }

        public void OnMapReady(GoogleMap map)
        {
            InvokeOnMapReadyBaseClassHack(map);
            NativeMap = map;
            NativeMap.SetInfoWindowAdapter(this);
            NativeMap.InfoWindowClick += OnInfoWindowClick;
            NativeMap.UiSettings.ZoomControlsEnabled = Map.HasZoomEnabled;
            NativeMap.UiSettings.ZoomGesturesEnabled = Map.HasZoomEnabled;
            NativeMap.UiSettings.ScrollGesturesEnabled = Map.HasScrollEnabled;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null) { NativeMap.InfoWindowClick -= OnInfoWindowClick; }
            if (e.NewElement == null) { return; }
            Pins = ((CustomMap)e.NewElement).CustomPins;
            Control.GetMapAsync(this);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (!e.PropertyName.Equals("VisibleRegion") || IsPinMarkersUpdated) { return; }
            NativeMap.Clear();

            IEnumerable<MarkerOptions> MarkerOptionsList = Pins.Select(entity => ToMarkerOptions(entity));
            foreach (MarkerOptions options in MarkerOptionsList) { NativeMap.AddMarker(options); }
            IsPinMarkersUpdated = true;
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);
            if (changed) { IsPinMarkersUpdated = false; }
        }

        private CustomPin GetCustomPin(Marker annotation)
        {
            Position position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);
            IEnumerable<CustomPin> candidates = Pins.Where(entity => entity.Position == position);
            if (candidates.Count() == 0) { return null; }
            return candidates.First();
        }

        private void InvokeOnMapReadyBaseClassHack(GoogleMap map)
        {
            IEnumerable<MethodInfo> candidates = typeof(MapRenderer)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(method => (method.IsFinal && method.IsPrivate))
                .Where(method => string.Equals(method.Name, "OnMapReady", StringComparison.Ordinal) ||
                    method.Name.EndsWith(".OnMapReady", StringComparison.Ordinal));
            if (candidates.Count() == 0) { return; }
            candidates.First().Invoke(this, new[] { map });
        }

        private void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            CustomPin customPin = GetCustomPin(e.Marker);
            if (customPin == null) { throw new Exception("Custom pin not found"); }
            customPin.OnInfoTapped(e);
        }

        private static LatLng ToLatLng(Position position) { return new LatLng(position.Latitude, position.Longitude); }

        private static MarkerOptions ToMarkerOptions(CustomPin customPin)
        {
            MarkerOptions options = new MarkerOptions();
            options.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.newpin));
            options.SetPosition(ToLatLng(customPin.Position));
            options.SetTitle(customPin.Label);
            return options;
        }
    }
}
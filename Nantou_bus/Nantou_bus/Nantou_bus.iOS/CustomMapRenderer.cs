using CoreGraphics;
using MapKit;
using Nantou_bus.iOS;
using Nantou_bus.UI.Map;
using System;
using System.Collections.Generic;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace Nantou_bus.iOS
{
    public class CustomMapRenderer : MapRenderer
    {
        UIView CustomPinView;
        List<CustomPin> CustomPins;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                MKMapView nativeMap = Control as MKMapView;
                nativeMap.GetViewForAnnotation = null;
                nativeMap.CalloutAccessoryControlTapped -= OnCalloutAccessoryControlTapped;
                nativeMap.DidSelectAnnotationView -= OnDidSelectAnnotationView;
                nativeMap.DidDeselectAnnotationView -= OnDidDeselectAnnotationView;
            }

            if (e.NewElement != null)
            {
                CustomMap formsMap = (CustomMap)e.NewElement;
                MKMapView nativeMap = Control as MKMapView;
                CustomPins = formsMap.CustomPinsTest;

                nativeMap.GetViewForAnnotation = GetViewForAnnotation;
                nativeMap.CalloutAccessoryControlTapped += OnCalloutAccessoryControlTapped;
                nativeMap.DidSelectAnnotationView += OnDidSelectAnnotationView;
                nativeMap.DidDeselectAnnotationView += OnDidDeselectAnnotationView;
            }
        }

        private MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            MKAnnotationView annotationView = null;

            if (annotation is MKUserLocation)
                return null;

            MKPointAnnotation anno = annotation as MKPointAnnotation;
            CustomPin customPin = GetCustomPin(anno);
            if (customPin == null)
            {
                throw new Exception("Custom pin not found");
            }

            annotationView = mapView.DequeueReusableAnnotation(customPin.Id);
            if (annotationView == null)
            {
                annotationView = new CustomMKAnnotationView(annotation, customPin.Id);
                annotationView.Image = UIImage.FromFile("newpin.png");
                annotationView.CalloutOffset = new CGPoint(0, 0);
                annotationView.LeftCalloutAccessoryView = new UIImageView(UIImage.FromFile("businfo2.png"));
                annotationView.RightCalloutAccessoryView = UIButton.FromType(UIButtonType.DetailDisclosure);
                ((CustomMKAnnotationView)annotationView).Id = customPin.Id;
            }
            annotationView.CanShowCallout = true;

            return annotationView;
        }

        private void OnCalloutAccessoryControlTapped(object sender, MKMapViewAccessoryTappedEventArgs e)
        {
            CustomMKAnnotationView customView = e.View as CustomMKAnnotationView;
            MKPointAnnotation anno = customView.Annotation as MKPointAnnotation;
            CustomPin customPin = GetCustomPin(anno);
            customPin.OnCalloutAccessoryTapped(e);
        }

        private void OnDidSelectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            CustomMKAnnotationView customView = e.View as CustomMKAnnotationView;
            CustomPinView = new UIView();
        }

        private void OnDidDeselectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            if (!e.View.Selected)
            {
                CustomPinView.RemoveFromSuperview();
                CustomPinView.Dispose();
                CustomPinView = null;
            }
        }

        private CustomPin GetCustomPin(MKPointAnnotation annotation)
        {
            Position position = new Position(annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);
            foreach (CustomPin pin in CustomPins)
            {
                if (pin.Pin.Position == position)
                {
                    return pin;
                }
            }
            return null;
        }
    }
}
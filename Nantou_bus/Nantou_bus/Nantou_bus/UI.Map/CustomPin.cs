using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Nantou_bus.UI.Map
{
    public class CustomPin : BindableObject
    {
        public static readonly BindableProperty TypeProperty = BindableProperty.Create("Type", typeof(PinType), typeof(CustomPin), default(PinType));

        public static readonly BindableProperty PositionProperty = BindableProperty.Create("Position", typeof(Position), typeof(CustomPin), default(Position));

        public static readonly BindableProperty AddressProperty = BindableProperty.Create("Address", typeof(string), typeof(CustomPin), default(string));

        public static readonly BindableProperty LabelProperty = BindableProperty.Create("Label", typeof(string), typeof(CustomPin), default(string));

        public string Address
        {
            get { return (string)GetValue(AddressProperty); }
            set { SetValue(AddressProperty, value); }
        }

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public Position Position
        {
            get { return (Position)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public PinType Type
        {
            get { return (PinType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public string Id { get; set; }

        public Pin Pin { get; set; }
        
        public event EventHandler InfoTapped;
        public void OnInfoTapped(EventArgs e){InfoTapped?.Invoke(this, e);}

        public event EventHandler MarkerClicked;
        public void OnMarkerClicked(EventArgs e) { MarkerClicked?.Invoke(this, e); }

        public event EventHandler CalloutAccessoryTapped;
        public void OnCalloutAccessoryTapped(EventArgs e) { { CalloutAccessoryTapped?.Invoke(this, e); } }



    }
}
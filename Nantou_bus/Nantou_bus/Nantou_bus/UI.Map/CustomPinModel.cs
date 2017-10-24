using System.ComponentModel;
using Xamarin.Forms.Maps;

namespace Nantou_bus.UI.Map
{
    public class CustomPinModel : INotifyPropertyChanged
    {
        string LabelVar;
        string AddressVar;
        Position PositionVar;
        PinType PintypeVar;

        public Position Position
        {
            get { return PositionVar; }
            set
            {
                if (PositionVar == value) { return; }
                PositionVar = value;
                OnPropertyChanged("Position");
            }
        }
        public string Label
        {
            get { return LabelVar; }
            set
            {
                if (LabelVar == value) { return; }
                LabelVar = value;
                OnPropertyChanged("Label");
            }
        }
        public PinType PinType
        {
            get { return PintypeVar; }
            set
            {
                if (PintypeVar == value) { return; }
                PintypeVar = value;
                OnPropertyChanged("Type");
            }
        }
        public string Address
        {
            get { return AddressVar; }
            set
            {
                if (AddressVar == value) { return; }
                AddressVar = value;
                OnPropertyChanged("Address");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

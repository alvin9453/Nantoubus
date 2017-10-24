using System.Collections.Generic;

namespace Nantou_bus.UI.Map
{
    public class CustomMap : Xamarin.Forms.Maps.Map
    {
        public IList<CustomPin> CustomPins { get; set; }

        public List<CustomPin> CustomPinsTest { get; set; }

        public CustomMap()
        {
            MapType = Xamarin.Forms.Maps.MapType.Street;
            IsShowingUser = true;
            CustomPins = new List<CustomPin>();
            CustomPinsTest = new List<CustomPin>();
        }
    }
}

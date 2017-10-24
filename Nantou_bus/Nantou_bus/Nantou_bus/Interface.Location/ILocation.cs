using System;

namespace Nantou_bus.Interface.Location
{
    public interface ILocation
    {
        void ObtainMyLocation();
        event EventHandler<ILocationEventArgs> locationObtained;
    }
    public interface ILocationEventArgs
    {
        double latitude { get; set; }
        double longitude { get; set; }
    }
}

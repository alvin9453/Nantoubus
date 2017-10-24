using MapKit;

namespace Nantou_bus.iOS
{
    public class CustomMKAnnotationView : MKAnnotationView
    {
        public string Id { get; set; }

        public CustomMKAnnotationView(IMKAnnotation annotation, string id) : base(annotation, id)
        {
        }
    }
}

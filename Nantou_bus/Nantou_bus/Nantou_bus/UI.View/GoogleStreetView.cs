namespace Nantou_bus.UI.View
{
    public class GoogleStreetView : Xamarin.Forms.Image
    {
        private const string BASE_URL = "https://maps.googleapis.com/maps/api/streetview?size=1080x1080&location={0},%20{1}&key={2}";

        private const string GOOGLE_MAP_KEY = "AIzaSyDUUf5gsUvFYC-s7D6iCAUvn19Yh09rdMM";

        public GoogleStreetView(double latitude, double longitude)
        {
            HeightRequest = 270;
            Source = string.Format(BASE_URL, latitude, longitude, GOOGLE_MAP_KEY);
        }
    }
}

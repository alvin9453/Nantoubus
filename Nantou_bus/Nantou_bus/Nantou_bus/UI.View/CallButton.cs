using Xamarin.Forms;

namespace Nantou_bus.UI.View
{
    public class CallButton : Button
    {
        public CallButton()
        {
            HorizontalOptions = LayoutOptions.End;
            Image = "call2.png";
            WidthRequest = 180;
        }
    }
}

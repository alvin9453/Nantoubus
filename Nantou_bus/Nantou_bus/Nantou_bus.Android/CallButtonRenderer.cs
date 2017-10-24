using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using Nantou_bus.Droid;
using Nantou_bus.UI.View;
using Android.Net;

[assembly: ExportRenderer(typeof(CallButton), typeof(CallButtonRenderer))]
namespace Nantou_bus.Droid
{
    class CallButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null) { return; }
            Control.Click += delegate
            {
                Uri uri = Uri.Parse("tel:" + Control.Text);
                Intent intent = new Intent(Intent.ActionDial, uri);
                Context.StartActivity(intent);
            };
        }
    }
}
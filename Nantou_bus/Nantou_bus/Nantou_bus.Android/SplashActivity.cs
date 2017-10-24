using Android.App;
using Android.Support.V7.App;

namespace Nantou_bus.Droid
{
    [Activity(Label = "南投客運通", Icon = "@drawable/busicon", Theme = "@style/splashscreen", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(typeof(MainActivity));
        }
    }
}
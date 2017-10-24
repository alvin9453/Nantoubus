using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Nantou_bus
{
    public class AboutUsPage : ContentPage
    {
        public AboutUsPage()
        {
            Title = "關於我們";
            var web = new WebView
            {
                Source = "http://ms14.voip.edu.tw/~wei0923/Project/iBus/AboutUs/AboutUs.html"
            };

            Content = web;
        }
    }
}
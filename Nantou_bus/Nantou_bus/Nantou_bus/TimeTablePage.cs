using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Nantou_bus
{
	public class TimeTablePage : ContentPage
	{
		public TimeTablePage (String RouteID,String SubRouteName)
		{
            String last = SubRouteName[SubRouteName.Length - 1].ToString();
            var web = new WebView
            {
                Source = "http://web.taiwanbus.tw/eBUS/subsystem/Timetable/TimeTableAPIByWeek.aspx?inputType=R01&RouteId=" + RouteID + "&RouteBranch=" + last +"&SearchDate=2017/07/26"
            };

            Content = web;
        }
	}
}
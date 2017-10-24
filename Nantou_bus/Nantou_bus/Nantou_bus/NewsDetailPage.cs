using Nantou_bus.Model.TransportData;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Nantou_bus
{
    public class NewsDetailPage : ContentPage
    {
        public NewsDetailPage(String newsId)
        {
            List<News> catchNewsID = News.RetrieveFromJson(string.Format("http://www.taiwanbus.tw/app_api/Marquee_N.ashx?id={0}", newsId));
            StackLayout stack = new StackLayout { Spacing = 10, Margin = 10 };
            stack.Children.Add(new Label());
            if (catchNewsID.Count != 0)
            {
                String news_title = catchNewsID[0].title;
                String news_content = catchNewsID[0].content;
                String news_from = catchNewsID[0].name;
                stack.Children.Add(new Label { TextColor = Color.Red, Text = news_title, FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)) });
                stack.Children.Add(new Label { TextColor = Color.Black, Text = news_content, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)) });
                stack.Children.Add(new Label { Text = " " });
                stack.Children.Add(new Label { TextColor = Color.Gray, Text = "資料來源:" + news_from, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) });
            }
            else
            {
                String news_title = "資料錯誤";
                String news_content = "目前無資料來源可以顯示";
                stack.Children.Add(new Label { TextColor = Color.Red, Text = news_title, FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)) });
                stack.Children.Add(new Label { TextColor = Color.Black, Text = news_content, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)) });
            }
            ScrollView scroll = new ScrollView { Content = stack };
            Content = scroll;
        }
    }
}
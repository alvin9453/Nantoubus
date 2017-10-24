using Nantou_bus.Model.TransportData;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Nantou_bus
{
    public class NewsPage : ContentPage
	{
        public NewsPage()
        {
            Title = "最新消息";
            StackLayout stack = new StackLayout { Spacing = 10, Margin = 10 };
            PopulateNews();
        }

        private void PopulateNews()
        {
            IEnumerable<News> catchNews = News.RetrieveFromJson("http://www.taiwanbus.tw/app_api/New_N.ashx");
            TableView table = new TableView();
            table.Intent = TableIntent.Form;
            TableRoot troot = new TableRoot();
            table.Root = troot;
            TableSection section = new TableSection();
            troot.Add(section);
            foreach(News catchNew in catchNews)
            {
                TextCell newsCell = new TextCell();
                newsCell.Text = catchNew.Updatetime;
                newsCell.TextColor = Color.Red;
                newsCell.Detail = catchNew.title;
                newsCell.DetailColor = Color.Black;
                newsCell.Tapped += (object sender, EventArgs e) =>
                {
                    this.Navigation.PushAsync(new NewsDetailPage(catchNew.id));
                };
                section.Add(newsCell);
            }
            Content = table;
        }
	}
}
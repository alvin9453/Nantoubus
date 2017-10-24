using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Nantou_bus
{
    public partial class MainPage : MasterDetailPage
    {
        private ListView MenuItems { get { return (Master as MasterPage).MenuItems; } }

        private static IDictionary<string, Type> MenuToDetailPage = new Dictionary<string, Type>()
        {
            { "地圖查找", typeof(MapPage) },
            { "路線明細", typeof(NumberSearch) },
            { "起訖站查詢", typeof(SearchHistory) },
            { "設定預存位置", typeof(PreSet) },
            { "最新消息", typeof(NewsPage) },
            { "快速聯繫", typeof(ContactPage) },
            { "關於我們", typeof(AboutUsPage) },
        };

        public MainPage()
        {
            InitializeComponent();
            Master = new MasterPage();
            Detail = new NavigationPage(new MapPage());
            MenuItems.ItemSelected += OnItemSelected;
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            MasterPageItem item = e.SelectedItem as MasterPageItem;
            if (item == null) { return; }
            Type contentPageType = (MenuToDetailPage.ContainsKey(item.Title)) ? MenuToDetailPage[item.Title] : typeof(MapPage);
            Page contentPage = (Page)Activator.CreateInstance(contentPageType);
            Detail = new NavigationPage(contentPage);
            PostItemSelected();
        }

        private void PostItemSelected()
        {
            MenuItems.SelectedItem = null;
            IsPresented = false;
        }
    }
}
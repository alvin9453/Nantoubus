using System.Collections.Generic;
using Xamarin.Forms;

namespace Nantou_bus
{
    public partial class MasterPage : ContentPage
    {
        public ListView MenuItems { get { return _MenuItems; } }

        public MasterPage()
        {
            InitializeComponent();
            InitMenuItems();
        }

        private void InitMenuItems()
        {
            _MenuItems.ItemsSource = new List<MasterPageItem>()
            {
                new MasterPageItem() { Title = "地圖查找", Icon = "\uf279" },
                new MasterPageItem() { Title = "起訖站查詢", Icon = "\uf277" },
                new MasterPageItem() { Title = "路線明細", Icon = "\uf124" },
                new MasterPageItem() { Title = "設定預存位置", Icon = "\uf015" },
                new MasterPageItem() { Title = "最新消息", Icon = "\uf1ea" },
                new MasterPageItem() { Title = "快速聯繫", Icon = "\uf2a0" },
                new MasterPageItem() { Title = "關於我們" }
            };
        }
    }
}
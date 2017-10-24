using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Nantou_bus.Model;

namespace Nantou_bus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PreSet : ContentPage
    {
        public PreSet()
        {
            InitializeComponent();
            HomeEntry.Text = GetLastCreateName<MyRecord>();
            OfficeEntry.Text = GetLastCreateName<OfficeRecord>();
            SchoolEntry.Text = GetLastCreateName<SchoolRecord>();
        }

        public void HomeButton_Clicked(object sender, EventArgs e)
        {
            OnClickSaveButton<MyRecord>(HomeEntry.Text);
        }

        public void OfficeButton_Clicked(object sender, EventArgs e)
        {
            OnClickSaveButton<OfficeRecord>(OfficeEntry.Text);
        }

        public void SchoolButton_Clicked(object sender, EventArgs e)
        {
            OnClickSaveButton<SchoolRecord>(SchoolEntry.Text);
        }

        private String GetLastCreateName<T>() where T : AbstractRecord, new()
        {
            IEnumerable<T> existed = MapPage.localDatabase.List<T>();
            if (existed.Count() > 0) { return existed.Last().Name; }
            else { return "尚未設定站牌"; }
        }

        private void OnClickSaveButton<T>(String name) where T : AbstractRecord, new()
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                DisplayAlert("系統提醒", "請輸入站牌名稱", "重新輸入");
                return;
            }
            T instance = Activator.CreateInstance<T>();
            instance.Name = name;
            MapPage.localDatabase.Create<T>(instance);
            DisplayAlert("系統提醒", "設定成功", "確定");
        }
    }
}
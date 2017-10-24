using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

using Nantou_bus.Model;
using Nantou_bus.Model.TransportData;
using Nantou_bus.UI.View;

namespace Nantou_bus
{
    public class ContactPage : ContentPage
    {
        private Layout<Xamarin.Forms.View> PageLayout;

        private OperatorPicker OperatorPicker;

        private ScrollView ResultView = new ScrollView() { WidthRequest = 350 };

        private static Label MessageLabel = new Label
        {
            HorizontalOptions = LayoutOptions.StartAndExpand,
            FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
            Text = "\n查詢客運之管轄監理單位 : ",
        };

        private static IList<Operator> Operators = new List<Operator>()
        {
            new Operator("53", "總達客運") { Authority = "臺中區監理所", Phone = "0800-021-258", Tel = "04-26912011" },
            new Operator("54", "全航客運") { Authority = "臺中區監理所", Phone = "04-22129715", Tel = "04-26912011" },
            new Operator("56", "台中客運") { Authority = "臺中區監理所", Phone = "0800-800-126", Tel = "04-26912011" },
            new Operator("58", "豐原客運") { Authority = "臺中區監理所", Phone = "0800-034-175", Tel = "04-26912011" },
            new Operator("60", "彰化客運") { Authority = "臺中區監理所", Phone = "04-7225111", Tel = "04-26912011" },
            new Operator("61", "員林客運") { Authority = "臺中區監理所", Phone = "0800-785-688", Tel = "04-26912011" },
            new Operator("62", "豐榮客運") { Authority = "臺中區監理所", Phone = "0800-280-008", Tel = "04-26912011" },
            new Operator("63", "杉林溪遊樂事業" ) { Authority = "臺中區監理所", Phone = "049-2611211", Tel = "04-26912011" },
            new Operator("64", "南投客運") { Authority = "臺中區監理所", Phone = "049-2984031", Tel = "04-26912011" },
            new Operator("21", "臺西客運") { Authority = "嘉義區監理所", Phone = "0800-200-079", Tel = "05-3623939" },
            new Operator("34", "統聯客運") { Authority = "臺北區監理所", Phone = "0800-241-560", Tel = "02-26884366" },
            new Operator("45", "國光客運") { Authority = "臺北區監理所", Phone = "0800-010-138", Tel = "02-26884366" },
            new Operator("46", "花蓮客運") { Authority = "臺北區監理所", Phone = "0800-322-816", Tel = "02-26884366" },
            
        };

        public ContactPage() : base()
        {
            Title = "快速聯繫";
            PageLayout = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 10,
                Margin = 10,
            };
            OperatorPicker = CreateOperatorPicker();
            PageLayout.Children.Add(MessageLabel);
            PageLayout.Children.Add(OperatorPicker);
            PageLayout.Children.Add(ResultView);
            Content = PageLayout;
        }

        public ContactPage(string routeID) : this() { PopulatePickerResult(routeID); }

        private OperatorPicker CreateOperatorPicker()
        {
            OperatorPicker picker = new OperatorPicker(Operators);
            picker.SelectedIndexChanged += (sender, args) => { ResultView.Content = picker.CreateContactInfoOnPick(); };
            return picker;
        }

        private void PopulatePickerResult(string routeID)
        {
            if (routeID == null) { return; }
            IList<BusRoute> routes = BusRoute.RetrieveFromJson("http://ptx.transportdata.tw/MOTC/v2/Bus/Route/InterCity/" + routeID + "?$format=JSON");
            if (routes.Count == 0) { return; }
            OperatorPicker.SelectedItem = Operators.Where(entity => string.Equals(entity.ID, routes[0].OperatorIDs[0])).First().Name;
            ResultView.Content = OperatorPicker.CreateContactInfoOnPick();
        }
    }
}
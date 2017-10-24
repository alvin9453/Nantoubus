using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

using Nantou_bus.Model;
using Nantou_bus.Model.TransportData;

namespace Nantou_bus.UI.View
{
    public class OperatorPicker : Picker
    {
        public string SelectedName { get; private set; }

        private IEnumerable<Operator> Operators;

        public OperatorPicker(IEnumerable<Operator> operators)
        {
            Title = "-- 請選擇客運業者 --    ﹀";
            HorizontalOptions = LayoutOptions.FillAndExpand;
            Operators = operators;
            foreach (Operator entity in Operators) { Items.Add(entity.Name); }
        }

        public Layout CreateContactInfoOnPick()
        {
            StackLayout result = new StackLayout();
            if (SelectedIndex == -1)
            {
                SelectedName = "";
                return result;
            }

            SelectedName = Items[SelectedIndex];
            Operator selected = Operators.Where(entity => string.Equals(entity.Name, SelectedName)).First();

            Label operatorName = CreateOperatorNameLabel(selected.Name);
            Layout operatorContact = new ContactInfoView(selected.Phone);
            Layout authorityContact = CreateAuthorityContact(selected.Authority, selected.Tel);

            result.Children.Add(operatorName);
            result.Children.Add(operatorContact);
            if (authorityContact != null) { result.Children.Add(authorityContact); }
            return result;
        }

        private static Layout CreateAuthorityContact(string authorityName, string authorityTel)
        {
            ContactInfoView contactInfo = new ContactInfoView(authorityTel, authorityName);
            return contactInfo;
        }

        private static Label CreateOperatorNameLabel(string name)
        {
            Label label = new Label
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                Text = name + "：",
            };
            return label;
        }
    }
}

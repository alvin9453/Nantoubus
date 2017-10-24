using Xamarin.Forms;

namespace Nantou_bus.UI.View
{
    public class ContactInfoView : StackLayout
    {
        private static Label MessageLabel = CreateMessageLabel();

        private string Phone;

        static ContactInfoView() { MessageLabel.Text = "聯絡電話"; }

        public ContactInfoView(string phone)
        {
            Init(phone);
            Children.Add(CreateContactWindowImage());
            Children.Add(MessageLabel);
            Children.Add(CreateCallButton());
        }

        public ContactInfoView(string phone, string message)
        {
            Init(phone);
            Label messageLabel = CreateMessageLabel();
            messageLabel.Text = message;

            Children.Add(CreateContactWindowImage());
            Children.Add(messageLabel);
            Children.Add(CreateCallButton());
        }

        private static Image CreateContactWindowImage()
        {
            return new Image
            {
                Source = "manager.png",
                WidthRequest = 40,
                HeightRequest = 40,
            };
        }

        private void Init(string phone)
        {
            Orientation = StackOrientation.Horizontal;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.Center;
            Phone = phone;
        }

        private Button CreateCallButton() { return new CallButton { Text = Phone }; }

        private static Label CreateMessageLabel()
        {
            return new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
            };
        }
    }
}

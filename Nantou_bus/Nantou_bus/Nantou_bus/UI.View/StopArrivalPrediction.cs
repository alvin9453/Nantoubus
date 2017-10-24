using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

using Nantou_bus.Model.TransportData;

namespace Nantou_bus.UI.View
{
    public class StopArrivalPrediction : StackLayout
    {
        private const string MESSAGE_PLANNED = "尚未發車";
        private const string MESSAGE_OUT_OF_SERVICE = "末班車駛離";

        private static ColumnDefinitionCollection DEFAULT_COLUMN_DEFINITION = new ColumnDefinitionCollection
        {
                new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(80, GridUnitType.Absolute) }
        };

        private static double DEFAULT_LABEL_FONTSIZE = Device.GetNamedSize(NamedSize.Default, typeof(Label));

        private static RowDefinitionCollection DEFAULT_ROW_DEFINITION = new RowDefinitionCollection
        {
            new RowDefinition{Height = new GridLength(64,GridUnitType.Absolute)}
        };

        private static double LARGE_LABEL_FONTSIZE = Device.GetNamedSize(NamedSize.Large, typeof(Label));
        
        private static double SMALL_LABEL_FONTSIZE = Device.GetNamedSize(NamedSize.Small, typeof(Label));

        public StopArrivalPrediction(StopOfRoute stopsOfSingleRoute, IEnumerable<RoutePrediction> predictions) : base()
        {
            Spacing = 3;
            Padding = new Thickness(20, 20, 20, 0);
            List<string> displayedPlateNumbers = new List<string>();
            foreach (StopOfRoute.Stop stop in stopsOfSingleRoute.Stops)
            {
                string stopName = stop.StopName.Zh_tw;
                IEnumerable<RoutePrediction> candidatePredictions = predictions.Where(prediction => string.Equals(stopName, prediction.StopName.Zh_tw));
                if (candidatePredictions.Count() != 1) { continue; }
                RoutePrediction currentPrediction = candidatePredictions.First();
                PopulatePredictionViewByStop(stopName, currentPrediction, displayedPlateNumbers);
            }
        }

        private Xamarin.Forms.View CreatArrivalMinutesView(int minutesToArrival)
        {
            Xamarin.Forms.View minutesView = new Label()
            {
                FontSize = LARGE_LABEL_FONTSIZE,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = GetArrivalMinutesColor(minutesToArrival),
                Text = minutesToArrival.ToString()
            };
            return minutesView;
        }

        private Xamarin.Forms.View CreateDescriptionView(string description)
        {
            Xamarin.Forms.View descriptionView = new Label()
            {
                HorizontalOptions = LayoutOptions.Center,
                FontSize = SMALL_LABEL_FONTSIZE,
                TextColor = Color.DimGray,
                Text = description
            };
            return descriptionView;
        }

        private Xamarin.Forms.View CreateStopNameView(string stopName)
        {
            Xamarin.Forms.View stopNameView = new Label()
            {
                FontSize = DEFAULT_LABEL_FONTSIZE,
                HorizontalOptions = LayoutOptions.Fill,
                TextColor = Color.DimGray,
                Text = stopName
            };
            return stopNameView;
        }

        private Color GetArrivalMinutesColor(int minutes)
        {
            if (minutes <= 5) { return Color.DarkRed; }
            if (minutes <= 15) { return Color.GreenYellow; }
            return Color.Black;
        }

        private void PopulatePredictionView(string stopName, string description)
        {
            Xamarin.Forms.View stopNameView = CreateStopNameView(stopName);
            Xamarin.Forms.View descriptionView = CreateDescriptionView(description);
            Grid innerGrid = new Grid { ColumnDefinitions = DEFAULT_COLUMN_DEFINITION,RowDefinitions = DEFAULT_ROW_DEFINITION };
            innerGrid.Children.Add(stopNameView);
            innerGrid.Children.Add(descriptionView, 2, 3, 0, 1);
            Children.Add(innerGrid);
        }

        private void PopulatePredictionView(string stopName, int secondsToArrival, string plateNumber = "")
        {
            if (string.Equals("-1", plateNumber)) { plateNumber = ""; }
            Xamarin.Forms.View stopNameView = CreateStopNameView(stopName);
            Xamarin.Forms.View plateNumberView = CreateDescriptionView(plateNumber);
            Xamarin.Forms.View minutesView = CreatArrivalMinutesView(secondsToArrival/60);
            Grid innerGrid = new Grid { ColumnDefinitions = DEFAULT_COLUMN_DEFINITION ,RowDefinitions = DEFAULT_ROW_DEFINITION };
            innerGrid.Children.Add(stopNameView);
            innerGrid.Children.Add(plateNumberView, 1, 2, 0, 1);
            innerGrid.Children.Add(minutesView, 2, 3, 0, 1);
            Children.Add(innerGrid);
        }

        private void PopulatePredictionViewByStop(string stopName, RoutePrediction prediction, List<string> displayedPlateNumbers)
        {
            int secondsToArrival = prediction.EstimateTime;
            string plateNumber = prediction.PlateNumb;

            if(plateNumber == "-1")
            {
                if (prediction.IsLastBus) { PopulatePredictionView(stopName, MESSAGE_OUT_OF_SERVICE); }
                else { PopulatePredictionView(stopName, MESSAGE_PLANNED); }
            }
            else if (displayedPlateNumbers.Contains(plateNumber)) { PopulatePredictionView(stopName, secondsToArrival); }
            else
            {
                PopulatePredictionView(stopName, secondsToArrival, plateNumber);
                displayedPlateNumbers.Add(plateNumber);
            }
        }
    }
}

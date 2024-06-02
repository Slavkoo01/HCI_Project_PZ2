using NetworkService.CustomControls;
using NetworkService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetworkService.Views
{
    /// <summary>
    /// Interaction logic for MeasurmentView.xaml
    /// </summary>
    public partial class MeasurmentView : UserControl
    {
        private MeasurementViewModel _measurementVM = new MeasurementViewModel();
        public MeasurmentView()
        {
            InitializeComponent();
            DataContext = _measurementVM;
        }
        private double _previousHeight1 = 5; 
        private double _previousHeight2 = 5; 
        private double _previousHeight3 = 5; 
        private double _previousHeight4 = 5; 
        private double _previousHeight5 = 5; 
        

        private Color GetColorBasedOnHeight(double height)
        {
            if (height < 267)
                return ((SolidColorBrush)Application.Current.Resources["Blue"]).Color;
            else if (height >= 267 && height <= 437)
                return ((SolidColorBrush)Application.Current.Resources["Neutral"]).Color;
            else
                return ((SolidColorBrush)Application.Current.Resources["Rose"]).Color;
        }
        private void AnimateBackgroundColor(Button button, Color toColor)
        {
            SolidColorBrush originalBrush = (SolidColorBrush)button.Background;
            SolidColorBrush animatedBrush = new SolidColorBrush(originalBrush.Color); 
            button.Background = animatedBrush; 

            ColorAnimation animation = new ColorAnimation(toColor, TimeSpan.FromSeconds(0.2));
            animatedBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }
        private DoubleAnimation CreateHeightAnimation(double from, double to)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = from;
            animation.To = to;
            animation.Duration = TimeSpan.FromSeconds(0.2);
            return animation;
        }
       
        private void customRecG1_1_HeightChanged(object sender, RoutedEventArgs e)
        {
            CustomRectangle customRec = sender as CustomRectangle;
            if (customRec == null) return;

            DoubleAnimation animation = CreateHeightAnimation(_previousHeight1, customRec.Height);
            _previousHeight1 = customRec.Height;
            customRecG1_2.BeginAnimation(CustomRectangle.HeightProperty, animation);

            AnimateBackgroundColor(customRecG1_2, GetColorBasedOnHeight(customRec.Height));
        }

        private void customRecG2_1_HeightChanged(object sender, RoutedEventArgs e)
        {
            CustomRectangle customRec = sender as CustomRectangle;
            if (customRec == null) return;

            DoubleAnimation animation = CreateHeightAnimation(_previousHeight1, customRec.Height);
            _previousHeight2 = customRec.Height;
            customRecG2_2.BeginAnimation(CustomRectangle.HeightProperty, animation);

            AnimateBackgroundColor(customRecG2_2, GetColorBasedOnHeight(customRec.Height));
        }

        private void customRecG3_1_HeightChanged(object sender, RoutedEventArgs e)
        {
            CustomRectangle customRec = sender as CustomRectangle;
            if (customRec == null) return;

            DoubleAnimation animation = CreateHeightAnimation(_previousHeight1, customRec.Height);
            _previousHeight3 = customRec.Height;
            customRecG3_2.BeginAnimation(CustomRectangle.HeightProperty, animation);

            AnimateBackgroundColor(customRecG3_2, GetColorBasedOnHeight(customRec.Height));
        }

        private void customRecG4_1_HeightChanged(object sender, RoutedEventArgs e)
        {
            CustomRectangle customRec = sender as CustomRectangle;
            if (customRec == null) return;

            DoubleAnimation animation = CreateHeightAnimation(_previousHeight1, customRec.Height);
            _previousHeight4 = customRec.Height;
            customRecG4_2.BeginAnimation(CustomRectangle.HeightProperty, animation);

            AnimateBackgroundColor(customRecG4_2, GetColorBasedOnHeight(customRec.Height));
        }

        private void customRecG5_1_HeightChanged(object sender, RoutedEventArgs e)
        {
            CustomRectangle customRec = sender as CustomRectangle;
            if (customRec == null) return;

            DoubleAnimation animation = CreateHeightAnimation(_previousHeight1, customRec.Height);
            _previousHeight5 = customRec.Height;
            customRecG5_2.BeginAnimation(CustomRectangle.HeightProperty, animation);

            AnimateBackgroundColor(customRecG5_2, GetColorBasedOnHeight(customRec.Height));
        }
    }
}

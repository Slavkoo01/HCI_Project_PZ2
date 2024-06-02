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
    /// Interaction logic for DragDropCardView.xaml
    /// </summary>
    public partial class DragDropCardView : UserControl
    {
        private ServerViewModel _serverViewModel;
        private DragDropCardViewModel _dragDropCardViewModel;
        private Point _initialMouseOffset;
        public ServerViewModel ServerViewModel { get { return _serverViewModel; } set { _serverViewModel = value; } }

        public Point InitialMouseOffset { get { return _initialMouseOffset; } set {  _initialMouseOffset = value; } }


        public DragDropCardView(ServerViewModel serverViewModel,DisplayView displayView)
        {
            InitializeComponent();
            _dragDropCardViewModel = new DragDropCardViewModel(this, displayView);
            closeButton.DataContext = _dragDropCardViewModel;
            ServerViewModel = serverViewModel;
            DataContext = ServerViewModel;
            this.MouseDown += UserControl_MouseDown;
            this.MouseMove += UserControl_MouseMove;
        }

       
        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _initialMouseOffset = e.GetPosition(this);
            }
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                    DragDrop.DoDragDrop(this, this, DragDropEffects.Move);            
            }
        }
        private void AnimateBackgroundColor(Border border, Color toColor)
        {
            SolidColorBrush originalBrush = (SolidColorBrush)border.Background;
            SolidColorBrush animatedBrush = new SolidColorBrush(originalBrush.Color);
            border.Background = animatedBrush;

            ColorAnimation animation = new ColorAnimation(toColor, TimeSpan.FromSeconds(0.2));
            animatedBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }
        private void AnimateForegroundColor(Button button, Color toColor)
        {
            SolidColorBrush originalBrush = (SolidColorBrush)button.Foreground;
            SolidColorBrush animatedBrush = new SolidColorBrush(originalBrush.Color);
            button.Foreground = animatedBrush;

            ColorAnimation animation = new ColorAnimation(toColor, TimeSpan.FromSeconds(0.2));
            animatedBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }
        private Color GetColorBasedOnHeight(double height)
        {
            if (height < 267)
            {
                AnimateForegroundColor(valuePresenter, ((SolidColorBrush)Application.Current.Resources["Neutral"]).Color);
                return ((SolidColorBrush)Application.Current.Resources["Blue"]).Color;
            }
            else if (height >= 267 && height <= 437)
            {
                AnimateForegroundColor(valuePresenter, ((SolidColorBrush)Application.Current.Resources["Background"]).Color);
                return ((SolidColorBrush)Application.Current.Resources["Neutral"]).Color;
            }
                
            else
            {
                AnimateForegroundColor(valuePresenter, ((SolidColorBrush)Application.Current.Resources["Neutral"]).Color);
                return ((SolidColorBrush)Application.Current.Resources["Blue"]).Color;
            }
        }
        private void CustomRectangle_ContentChanged(object sender, RoutedEventArgs e)
        {
            AnimateBackgroundColor(valueBorder, GetColorBasedOnHeight(ServerViewModel.Value1));
        }

        
    }
}

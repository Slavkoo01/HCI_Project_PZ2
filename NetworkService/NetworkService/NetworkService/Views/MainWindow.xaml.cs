using NetworkService.Helper;
using NetworkService.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetworkService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double _expendedWindowWidth = 1580;
        private double _windowWidth = 1080;
        private bool IsWindowLoaded = false;
        private double width = 140;
        public MainWindowViewModel MainWindowViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            MainWindowViewModel = new MainWindowViewModel(this);
            DataContext = MainWindowViewModel;
            setVisibilityies();
            this.Loaded += MainWindow_Loaded;
            tgb_entities_Click(null,new RoutedEventArgs());
            

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            IsWindowLoaded = true;
            if (IsWindowLoaded)
            {
                tgb_help.IsChecked = true;
            }
        }

        public void OpenHelpAnimation()
        {
            DoubleAnimation widthAnimation = new DoubleAnimation
            {
                From = _windowWidth,
                To = _expendedWindowWidth,
                Duration = TimeSpan.FromSeconds(.2) 
            };

            this.BeginAnimation(Window.WidthProperty, widthAnimation);
        }
        public void CloseHelpAnimation()
        {
            DoubleAnimation widthAnimation = new DoubleAnimation
            {
                From = _expendedWindowWidth,
                To = _windowWidth,
                Duration = TimeSpan.FromSeconds(.2)
            };

            this.BeginAnimation(Window.WidthProperty, widthAnimation);
        }

        private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            // Set tooltip visibility

            if (Tg_Btn.IsChecked == true)
            {
                tt_entities.Visibility = Visibility.Collapsed;
                tt_display.Visibility = Visibility.Collapsed;
                tt_measurments.Visibility = Visibility.Collapsed;
                tt_help.Visibility = Visibility.Collapsed;
                tt_shutdown.Visibility = Visibility.Collapsed;
                
            }
            else
            {
                tt_entities.Visibility = Visibility.Visible;
                tt_display.Visibility = Visibility.Visible;
                tt_measurments.Visibility = Visibility.Visible;
                tt_help.Visibility = Visibility.Visible;
                tt_shutdown.Visibility = Visibility.Visible;
               
            }
        }
        private void Tg_Btn_Unchecked(object sender, RoutedEventArgs e)
        {
            mainFrmeRectangle.Visibility = Visibility.Collapsed;
        }

        private void Tg_Btn_Checked(object sender, RoutedEventArgs e)
        {
            mainFrmeRectangle.Visibility = Visibility.Visible;
        }

        private void BG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           Tg_Btn.IsChecked = false;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        public void tgb_shutdown_Checked()
        {
            pnl_ShutDown.Background = (SolidColorBrush)Application.Current.FindResource("Background");
            MainWindowViewModel.SaveCanvas();
            Close();
        }

        public void tgb_entities_Click(object sender, RoutedEventArgs e)
        {
            pnl_entities.Background = (SolidColorBrush)Application.Current.FindResource("Background");
            pnl_display.Background = (SolidColorBrush)Application.Current.FindResource("Card");
            pnl_measurments.Background = (SolidColorBrush)Application.Current.FindResource("Card");

            tgb_entities.Foreground = (SolidColorBrush)Application.Current.FindResource("Yellow");
            tgb_display.Foreground = (SolidColorBrush)Application.Current.FindResource("Neutral");
            tgb_mesurments.Foreground = (SolidColorBrush)Application.Current.FindResource("Neutral");
        }

        public void tgb_display_Click(object sender, RoutedEventArgs e)
        {
            pnl_entities.Background = (SolidColorBrush)Application.Current.FindResource("Card");
            pnl_display.Background = (SolidColorBrush)Application.Current.FindResource("Background");
            pnl_measurments.Background = (SolidColorBrush)Application.Current.FindResource("Card");

            tgb_entities.Foreground = (SolidColorBrush)Application.Current.FindResource("Neutral");
            tgb_display.Foreground = (SolidColorBrush)Application.Current.FindResource("Yellow");
            tgb_mesurments.Foreground = (SolidColorBrush)Application.Current.FindResource("Neutral");
        }

        public void tgb_mesurments_Click(object sender, RoutedEventArgs e)
        {
            pnl_entities.Background = (SolidColorBrush)Application.Current.FindResource("Card");
            pnl_display.Background = (SolidColorBrush)Application.Current.FindResource("Card");
            pnl_measurments.Background = (SolidColorBrush)Application.Current.FindResource("Background");

            tgb_entities.Foreground = (SolidColorBrush)Application.Current.FindResource("Neutral");
            tgb_display.Foreground = (SolidColorBrush)Application.Current.FindResource("Neutral");
            tgb_mesurments.Foreground = (SolidColorBrush)Application.Current.FindResource("Yellow");
        }

        private void tgb_shutdown_MouseEnter(object sender, MouseEventArgs e)
        {
            tgb_shutdown.Foreground = (SolidColorBrush)Application.Current.FindResource("Rose");
        }

        private void tgb_shutdown_MouseLeave(object sender, MouseEventArgs e)
        {
            tgb_shutdown.Foreground = (SolidColorBrush)Application.Current.FindResource("Neutral");
        }
        private void setVisibilityies()
        {
            tt_entities.Width = 0;
            tt_display.Width = 0;
            tt_measurments.Width = 0;
            tt_help.Width = 0;
            tt_shutdown.Width = 0;
        }
        private void tgb_help_Checked(object sender, RoutedEventArgs e)
        {
            if (IsWindowLoaded)
            {
                tt_entities.Width = 175;
                tt_display.Width = 175;
                tt_measurments.Width = 175;
                tt_help.Width = 175;
                tt_shutdown.Width = 175;
                foreach (var item in GlobalVar.buttons)
                {
                    item.Width = width;
                }
                foreach (var item in GlobalVar.toolTips)
                {
                    item.Width = 175;
                }
            }
        }
        private void tgb_help_Unchecked(object sender, RoutedEventArgs e)
        {

            if (IsWindowLoaded)
            {
                tt_entities.Width = 0;
                tt_display.Width = 0;
                tt_measurments.Width = 0;
                tt_help.Width = 0;
                tt_shutdown.Width = 0;
                foreach(var item in GlobalVar.buttons)
                {
                    item.Width = 0;
                }
                foreach (var item in GlobalVar.toolTips)
                {
                    item.Width = 0;
                }
            }
        }

        private void Main_Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                MainWindowViewModel.EntitiesView._entityVM.AddToEntityCollection();
            }
        }
    }
}

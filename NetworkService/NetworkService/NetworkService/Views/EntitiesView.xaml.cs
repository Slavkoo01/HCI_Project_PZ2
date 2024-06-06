using MVVMLight.Messaging;
using NetworkService.Helper;
using NetworkService.ViewModel;
using Notification.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetworkService.Views
{
    /// <summary>
    /// Interaction logic for EntitiesView.xaml
    /// </summary>
    public partial class EntitiesView : UserControl
    {
        private ToastNotification _toaNotification = new ToastNotification();
        private EntitiesViewModel _entityVM;
        private MainWindow mw;
        public EntitiesView(DisplayView displayView, MainWindow mw)
        {
            InitializeComponent();
            _entityVM = new EntitiesViewModel(displayView);
            this.mw = mw;   
            HelpButton.Click += HelpButton_Click;
            HelpButton.DataContext = mw.MainWindowViewModel;
            DataContext = _entityVM;
            GlobalVar.buttons.Add(HelpButton);
            GlobalVar.toolTips.Add(tt_search);
            GlobalVar.toolTips.Add(tt_submit);
            GlobalVar.toolTips.Add(tt_reset);
            GlobalVar.toolTips.Add(tt_delete);
            this.MouseMove += EntitiesView_MouseMove;
        }

        

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalVar.IsHelpOpen)
            {
                mw.CloseHelpAnimation();
                GlobalVar.IsHelpOpen = !GlobalVar.IsHelpOpen;
            }
            else
            {
                mw.OpenHelpAnimation();
                GlobalVar.IsHelpOpen = !GlobalVar.IsHelpOpen;
            }
        }

        private void Delete_btn_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = ServerListView.SelectedItems.Cast<ServerViewModel>().ToList();
            if (selectedItems.Count == 1)
            {
            var notification = _toaNotification.CreateDeleteToastNotification(this, "Are you certain you want to delete selected server?");
            Messenger.Default.Send<NotificationContent>(notification);
            }
            else if(selectedItems.Count > 1)
            {
                var notification = _toaNotification.CreateDeleteToastNotification(this, "Are you certain you want to delete selected servers?");
                Messenger.Default.Send<NotificationContent>(notification);
            }
            else
            {

            var notification = _toaNotification.CreateAnouncementToastNotification();
            Messenger.Default.Send<NotificationContent>(notification);
            }
        }
        public void Delete()
        {
            var selectedItems = ServerListView.SelectedItems.Cast<ServerViewModel>().ToList();

            foreach (var selectedItem in selectedItems)
            {
                _entityVM.DeleteServerBase(selectedItem);
            }
        }

        
        private void EntitiesView_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                var hitTestResult = VisualTreeHelper.HitTest(this, e.GetPosition(this));
                if(hitTestResult == null || !(hitTestResult.VisualHit is ListView)) 
                {
                    ServerListView.SelectedItems.Clear();
                }
            }
        }
    }
}

using MVVMLight.Messaging;
using NetworkService.Helper;
using NetworkService.Views;
using Notification.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace NetworkService.ViewModel
{
    public class DragDropCardViewModel : ViewModelBase
    {
        public ICommand CloseDragDropCommand => new CommandBase(execute => CloseDragDrop());
        private Canvas _canvas;
        private DragDropCardView _dragDropCard;
        private DisplayViewModel _displayViewModel;
        private ToastNotification _toaNotification = new ToastNotification();
        public DragDropCardViewModel(DragDropCardView dragDropCard, DisplayView displayView)
        {
            _canvas = displayView.Canvas;
            _dragDropCard = dragDropCard;
            _displayViewModel = displayView.DisplayViewModel;
        }

        private void CloseDragDrop()
        {
            var notification = _toaNotification.CreateDeleteCardToastNotification(this,"Are you certain you want to remove this card?");
            Messenger.Default.Send<NotificationContent>(notification);
        }
        public void Delete()
        {
            _canvas.Children.Remove(_dragDropCard);
            _dragDropCard.RemoveLines();
            _displayViewModel.AddNode(_dragDropCard.ServerViewModel);
        }
    }       
}

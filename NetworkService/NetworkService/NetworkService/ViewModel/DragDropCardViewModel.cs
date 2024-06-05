using NetworkService.Views;
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

        public DragDropCardViewModel(DragDropCardView dragDropCard, DisplayView displayView)
        {
            _canvas = displayView.Canvas;
            _dragDropCard = dragDropCard;
            _displayViewModel = displayView.DisplayViewModel;
        }

        private void CloseDragDrop()
        {
            _canvas.Children.Remove(_dragDropCard);
            _displayViewModel.AddNode(_dragDropCard.ServerViewModel);
        }
    }       
}

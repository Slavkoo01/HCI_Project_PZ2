using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.ViewModel
{
    public class MeasurementViewModel : ViewModelBase   
    {
        private ServerViewModel _selectedServer = new ServerViewModel();
        public ServerViewModel SelectedServer {  get { return _selectedServer; } set { _selectedServer = value; OnPropertyChanged(nameof(SelectedServer)); OnPropertyChanged(nameof(SelectedServerValue1)); } }
        public double SelectedServerValue1
        {
            get { return SelectedServer?.Value1 ?? 0; }
        }
        public ObservableCollection<ServerViewModel> MeasurementCollection { get; set; } = EntitiesViewModel.EntityColection;

        public MeasurementViewModel()
        {

        }
    }
}

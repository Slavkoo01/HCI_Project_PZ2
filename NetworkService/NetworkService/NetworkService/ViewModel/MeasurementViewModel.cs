using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NetworkService.ViewModel
{
   
    public class MeasurementViewModel : ViewModelBase   
    {
        private ServerViewModel _selectedServer = new ServerViewModel();
        public ServerViewModel SelectedServer {  get { return _selectedServer; } set { _selectedServer = value; OnPropertyChanged(nameof(SelectedServer)); } }

        public ObservableCollection<ServerViewModel> MeasurementCollection { get; set; } = EntitiesViewModel.EntityColection;

        public MeasurementViewModel()
        {

        }
    }
}

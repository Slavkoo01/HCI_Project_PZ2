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
        public ObservableCollection<ServerViewModel> MeasurementCollection { get; set; } = EntitiesViewModel.EntityColection;

        public MeasurementViewModel()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.ViewModel
{
    public class DisplayViewModel : ViewModelBase
    {
        public ObservableCollection<ServerViewModel> DisplayCollection { get; set; } = EntitiesViewModel.EntityColection; 

        public DisplayViewModel() 
        {
        }
    }
}

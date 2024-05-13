using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;




namespace NetworkService.ViewModel
{
    public class EntitiesViewModel : ViewModelBase
    {
        public static ObservableCollection<ServerViewModel> EntityColection { get; set; } = new ObservableCollection<ServerViewModel>();

        public EntitiesViewModel() 
        {
            EntityColection.Add(new ServerViewModel(new Server(1, "Server_01", "192.168.0.1", Types.WebServer)));
            EntityColection.Add(new ServerViewModel(new Server(2, "Server_02", "192.168.0.2", Types.FileServer)));
            EntityColection.Add(new ServerViewModel(new Server(3, "Server_03", "192.168.0.3", Types.DataBaseServer)));
            ServerViewModel sw = new ServerViewModel(new Server(4, "Server_04", "192.168.0.4", Types.DataBaseServer));
            EntityColection.Add(sw);
        }

    }
}

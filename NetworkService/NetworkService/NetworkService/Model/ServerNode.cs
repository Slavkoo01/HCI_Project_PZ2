using NetworkService.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NetworkService.Model
{
    public class ServerNode : ViewModelBase
    {
        private string _type;
        public string Type 
        {
            get { return _type; }
            set { _type = value; OnPropertyChanged(nameof(Type)); }
        }
        private ObservableCollection<ServerViewModel> _serverViewModels;
        public ObservableCollection<ServerViewModel> ServerViewModels 
            { 
                get { return _serverViewModels;  } 
                set { _serverViewModels = value; OnPropertyChanged(nameof(ServerViewModel)); }  
            }

        public ServerNode(Types type) 
        {
            Type = type.ToString();
            ServerViewModels = new ObservableCollection<ServerViewModel>();
        }

        public override string ToString()
        {
            string s = Type.ToString() + "\n\n";
           /* foreach(var item in serverViewModels)
            {
                s += item.ToString() + "\n";
            }*/
            return s;
        }
    }
}

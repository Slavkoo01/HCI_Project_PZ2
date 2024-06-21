using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace NetworkService.ViewModel
{
    public class DisplayViewModel : ViewModelBase
    {
        public ObservableCollection<ServerViewModel> DisplayCollection { get; set; } = EntitiesViewModel.EntityColection;
        
        private ObservableCollection<ServerNode> _displayNodes;
        public ObservableCollection<ServerNode> DisplayNodes
        {
            get { return _displayNodes; }
            set { _displayNodes = value; OnPropertyChanged(nameof(DisplayNodes)); }
        }
        public DisplayViewModel() 
        {
            GenerateDisplayNodes();           
        }

        private void GenerateDisplayNodes()
        {
            DisplayNodes = new ObservableCollection<ServerNode>();
            foreach (Types type in Enum.GetValues(typeof(Types)))
            {
                ServerNode sn = new ServerNode(type);
                foreach (ServerViewModel server in DisplayCollection)
                {
                    if (server.ServerType.Type == type)
                    {
                        sn.ServerViewModels.Add(server);
                    }
                }
                DisplayNodes.Add(sn);
            }
        }
        public void AddNode(ServerViewModel server)
        {
            foreach (var node in DisplayNodes)
            {
                if (server.ServerType.Type.ToString() == node.Type)
                {
                    node.ServerViewModels.Add(server);
                }
            }
        }
        public void RemoveNode(ServerViewModel server)
        {
            foreach(var node in DisplayNodes)
            {
                if(server.ServerType.Type.ToString() == node.Type)
                {
                    node.ServerViewModels.Remove(server);
                    break;
                }
            }
        }

    }
}

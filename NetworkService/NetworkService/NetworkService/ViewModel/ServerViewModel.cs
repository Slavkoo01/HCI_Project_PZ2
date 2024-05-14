using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NetworkService.ViewModel
{
    [Serializable]
    public class ServerViewModel : ViewModelBase
    {
        [XmlIgnore]
        private int? _id;
        [XmlIgnore]
        private int _value = 0;
        [XmlIgnore]
        private string _name;
        [XmlIgnore]
        private string _ipAddress;
        [XmlIgnore]
        private ServerTypeModel _serverType;
       

        public int? Id { get { return _id; } set { _id = value; OnPropertyChanged(nameof(Id)); } }
        [XmlIgnore]
        public int Value { get { return _value; } set { _value = value; OnPropertyChanged(nameof(Value)); } } 
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged(nameof(Name)); } }

        public string IpAddress { get { return _ipAddress; } set { _ipAddress = value; OnPropertyChanged(nameof(IpAddress)); } }

        public ServerTypeModel ServerType { get { return _serverType; } set { _serverType = value; OnPropertyChanged(nameof(ServerType)); } }

        public ServerViewModel(Server server)
        {
            Id = server.Id;
            Name = server.Name;
            IpAddress = server.IpAddress;
            ServerType = server.ServerType;
        }
        public ServerViewModel( )
        {
            _serverType = new ServerTypeModel();
        }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, IpAddress: {IpAddress} ServerType: " + ServerType.ToString();
        }
    }
}

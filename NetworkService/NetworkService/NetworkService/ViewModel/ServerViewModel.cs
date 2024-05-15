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
        private string _name;
        [XmlIgnore]
        private string _ipAddress;
        [XmlIgnore]
        private ServerTypeModel _serverType;
        [XmlIgnore]
        private int _value1 = 5;
        [XmlIgnore]
        private int _value2 = 5;
        [XmlIgnore]
        private int _value3 = 5;
        [XmlIgnore]
        private int _value4 = 5;
        [XmlIgnore]
        private int _value5 = 5;
        [XmlIgnore]
        private string _time1 = "--:--";
        [XmlIgnore]
        private string _time2 = "--:--";
        [XmlIgnore]
        private string _time3 = "--:--";
        [XmlIgnore]
        private string _time4 = "--:--";
        [XmlIgnore]
        private string _time5 = "--:--";
        

        [XmlIgnore]
        public string Time1 { get { return _time1; } set { _time1 = value; OnPropertyChanged(nameof(Time1)); } }
        [XmlIgnore]
        public string Time2 { get { return _time2; } set { _time2 = value; OnPropertyChanged(nameof(Time2)); } }
        [XmlIgnore]
        public string Time3 { get { return _time3; } set { _time3 = value; OnPropertyChanged(nameof(Time3)); } }
        [XmlIgnore]
        public string Time4 { get { return _time4; } set { _time4 = value; OnPropertyChanged(nameof(Time4)); } }
        [XmlIgnore]
        public string Time5 { get { return _time5; } set { _time5 = value; OnPropertyChanged(nameof(Time5)); } }


        [XmlIgnore]
        public int Value1 { get { return _value1; } set { _value1 = value; OnPropertyChanged(nameof(Value1)); } } 
        [XmlIgnore]
        public int Value2 { get { return _value2; } set { _value2 = value; OnPropertyChanged(nameof(Value2)); } }
        [XmlIgnore]
        public int Value3 { get { return _value3; } set { _value3 = value; OnPropertyChanged(nameof(Value3)); } }
        [XmlIgnore]
        public int Value4 { get { return _value4; } set { _value4 = value; OnPropertyChanged(nameof(Value4)); } } 
        [XmlIgnore]
        public int Value5 { get { return _value5; } set { _value5 = value; OnPropertyChanged(nameof(Value5)); } }

        public int? Id { get { return _id; } set { _id = value; OnPropertyChanged(nameof(Id)); } }
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged(nameof(Name)); } }

        public string IpAddress { get { return _ipAddress; } set { _ipAddress = value; OnPropertyChanged(nameof(IpAddress)); } }

        public ServerTypeModel ServerType { get { return _serverType; } set { _serverType = value; OnPropertyChanged(nameof(ServerType)); } }

        public int MapValue(int value)
        {           
            if(value >= 100) return 100;
            
            double mappedValue = 5 + (value / 100.0) * (588 - 5);

            int roundedValue = (int)Math.Round(mappedValue);

            return roundedValue;
        }
        public void NewValue(int value, string time)
        {
            Value5 = Value4; 
            Value4 = Value3; 
            Value3 = Value2; 
            Value2 = Value1; 
            Value1 = MapValue(value);

            Time5 = Time4;
            Time4 = Time3;
            Time3 = Time2;
            Time2 = Time1;
            Time1 = time;

        }
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

        public string ServerToString()
        {
            return $"Id: {Id}, Name: {Name}, IpAddress: {IpAddress} ServerType: " + ServerType.ToString() + $"\n Val1: {Value1}  time1: {Time1}\n Val2: {Value2} time2: {Time2}\n Val3: {Value3} time3: {Time3}\n Val4: {Value4} time4: {Time4}\n Val5: {Value5} time5: {Time5}";
        }
        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, IpAddress: {IpAddress} ServerType: {ServerType.Type}" ;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{
    public class Server
    {
        private int _id;
        private string _name;   
        private string _ipAddress;
        private ServerTypeModel _serverType;

        public Server(int id, string name, string ipAddress, Types serverType)
        {
            Id = id;
            Name = name;
            IpAddress = ipAddress;
            ServerType = new ServerTypeModel(serverType);
        }

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public string IpAddress { get => _ipAddress; set => _ipAddress = value; }
        public ServerTypeModel ServerType { get => _serverType; set => _serverType = value; }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, IpAddress: {IpAddress}, ServerType: {ServerType}";
        }
    }
}

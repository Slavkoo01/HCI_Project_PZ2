using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{
    public enum Types
    {
        FileServer,
        WebServer,
        DataBaseServer

    }
    public class ServerTypeModel
    {
        public Types? Type { get; set; }
        public string ServerTypeImage { get; set; }

        public ServerTypeModel(Types? serverType)
        {
            Type = serverType;
            switch(serverType)
            {
                case Types.FileServer:
                    ServerTypeImage = "/Assets/FileServer.png";
                    break;
                case Types.WebServer:
                    ServerTypeImage = "/Assets/WebServer.png";
                    break;
                case Types.DataBaseServer:
                    ServerTypeImage = "/Assets/DataBaseServer.png";
                    break;
            }
        }
        public ServerTypeModel()
        {
           
        }

        public override string ToString()
        {
            return $"ServerType: {Type}, ServerTypeImage: {ServerTypeImage}";
        }
    }
}

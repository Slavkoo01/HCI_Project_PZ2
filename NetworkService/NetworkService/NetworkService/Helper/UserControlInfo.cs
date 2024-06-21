using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Helper
{
    [Serializable]
    public class UserControlInfo
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public int? Id { get; set; }
    }
}

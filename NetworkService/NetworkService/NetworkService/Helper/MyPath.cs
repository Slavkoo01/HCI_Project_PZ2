using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NetworkService.Helper
{
    public static class MyPath
    {
        public static string MyDirectoryPath()
        {
            return Directory.GetParent(Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).FullName).FullName).FullName;
        }

        public static string MyXMLData()
        {
            return Path.Combine(Path.Combine(MyDirectoryPath(), "Repositories"), "XMLData");
        }   
       
    }
}

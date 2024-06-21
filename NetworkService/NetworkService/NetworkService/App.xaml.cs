using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using NetworkService.Repositories;
using NetworkService.ViewModel;
namespace NetworkService
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            XMLFiles.LoadDataFromXML();
            MainWindow = new MainWindow();
            
            
            
            
            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}

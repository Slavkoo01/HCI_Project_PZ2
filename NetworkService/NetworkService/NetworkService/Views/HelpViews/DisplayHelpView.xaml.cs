using NetworkService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetworkService.Views.HelpViews
{
    /// <summary>
    /// Interaction logic for DisplayHelpView.xaml
    /// </summary>
    public partial class DisplayHelpView : UserControl
    {
     
        public DisplayHelpView()
        {
            DataContext = this;
            InitializeComponent();
        }
    }
}

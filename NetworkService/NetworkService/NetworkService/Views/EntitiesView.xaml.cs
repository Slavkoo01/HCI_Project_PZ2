using NetworkService.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace NetworkService.Views
{
    /// <summary>
    /// Interaction logic for EntitiesView.xaml
    /// </summary>
    public partial class EntitiesView : UserControl
    {
        ListView listview;
        private EntitiesViewModel _entityVM;
        public EntitiesView()
        {
            _entityVM = new EntitiesViewModel();
            InitializeComponent();
            DataContext = _entityVM;
        }

        private void Delete_btn_Click(object sender, RoutedEventArgs e)
        {
           var selectedItems = ServerListView.SelectedItems.Cast<ServerViewModel>().ToList();
            
            foreach (var selectedItem in selectedItems)
            {
                EntitiesViewModel.EntityColection.Remove(selectedItem);
            }   
        }
    }
}

using NetworkService.Helper;
using NetworkService.Model;
using NetworkService.ViewModel.Base;
using NetworkService.ViewModel.Form;
using NetworkService.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;





namespace NetworkService.ViewModel
{
    public class EntitiesViewModel : ViewModelBase
    {
        #region Properties
        private ServerForm _server;
        private FilterForm _serverFilter;
        private DisplayViewModel _displayViewModel;
        private Canvas _canvas;
        private ObservableCollection<ServerViewModel> _listViewCollection;
        
        public ServerForm Server { get { return _server; } set { _server = value; OnPropertyChanged(nameof(Server)); } }
        public FilterForm ServerFilter { get { return _serverFilter; } set { _serverFilter = value; OnPropertyChanged(nameof(ServerFilter)); } }
        public ObservableCollection<ServerViewModel> ListViewCollection { get { return _listViewCollection; } set { _listViewCollection = value; OnPropertyChanged(nameof(ListViewCollection)); } }
        public ObservableCollection<Types> TypeCollection { get; set; }     

        public static ObservableCollection<ServerViewModel> EntityColection { get; set; } = new ObservableCollection<ServerViewModel>();
        #endregion
        private void PopulateTypeCollection()
        {
            TypeCollection = new ObservableCollection<Types>();
            foreach (Types type in Enum.GetValues(typeof(Types)))
            {
                TypeCollection.Add(type);
            }
        }

        #region Commands
        public ICommand SubmitCommand => new CommandBase(execute => AddToEntityCollection());
        public ICommand SearchCommand => new CommandBase(execute => ApplyFilters());
        public ICommand ResetCommand => new CommandBase(execute => ResetFilter());

        public void DeleteServerBase(ServerViewModel serverViewModel)
        {
            EntityColection.Remove(serverViewModel);
            _displayViewModel.RemoveNode(serverViewModel);

            RemoveUserControlFromCanvas(serverViewModel);
        }

        private void RemoveUserControlFromCanvas(ServerViewModel serverViewModel)
        {
            UserControl userControlToRemove = FindUserControlByDataContext(serverViewModel);
            if (userControlToRemove != null)
            {
                _canvas.Children.Remove(userControlToRemove);
            }
        }

        private UserControl FindUserControlByDataContext(ServerViewModel serverViewModel)
        {
            foreach (var child in _canvas.Children)
            {
                if (child is UserControl userControl && userControl.DataContext == serverViewModel)
                {
                    return userControl;
                }
            }
            return null;
        }
        private void AddToEntityCollection()
        {
            if (Server.Validate())
            {
                ServerViewModel temp = Server.CreateViewModel();
                EntityColection.Add(temp);
                _displayViewModel.AddServer(temp);
                GlobalVar.IsSaved = false;
                Server.ResetValues();
            }
        }
        private void ApplyFilters()
        {
            if (ServerFilter.IsValid())
            {
                ListViewCollection = ServerFilter.Filter();
                ServerFilter.ResetFilterOnSerach();
            }
        }
      
        private void ResetFilter()
        {
            ListViewCollection = ServerFilter.ResetFilter();
        }
         #endregion
        
        public EntitiesViewModel(DisplayView displayView) 
        {
            _canvas = displayView.Canvas;
            _displayViewModel = displayView.DisplayViewModel;
            Server = new ServerForm();
            ServerFilter = new FilterForm();
            ListViewCollection =  EntityColection;
            PopulateTypeCollection();                      
        }

    }
}

using NetworkService.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;




namespace NetworkService.ViewModel
{
    public class EntitiesViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private ErrorViewModel _errorViewModel;

        private ServerViewModel _server = new ServerViewModel();
        public static ObservableCollection<ServerViewModel> EntityColection { get; set; } = new ObservableCollection<ServerViewModel>();

       

        public ServerViewModel Server
        {
            get { return _server; }
            set
            {
                _server = value;
                OnPropertyChanged(nameof(ServerViewModel));
            }
        }
        #region ErrorHandling
        private void ErrorViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
        }
        public IEnumerable GetErrors(string propertyName)
        {
            return _errorViewModel.GetErrors(propertyName);
        }
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => _errorViewModel.HasErrors;

        #endregion

        private bool Validate()
        {
            return true;
        }

        public EntitiesViewModel() 
        {
            


        }

    }
}

using NetworkService.Helper;
using NetworkService.Model;
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
    public class EntitiesViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        #region Properties
        private string _id;
        private string _name;
        private string _ipAddress;
        private Types? _serverType;
        private List<ServerViewModel> myListView;
        private ObservableCollection<ServerViewModel> _filteredServers;
        public ObservableCollection<ServerViewModel> FilteredServers
        {
            get => _filteredServers;
            set
            {
                _filteredServers = value;
                OnPropertyChanged(nameof(FilteredServers));
            }
        }

        private string _textBoxFilter;
        public string TextBoxFilter
        {
            get => _textBoxFilter;
            set
            {
                _textBoxFilter = value;
                OnPropertyChanged(nameof(TextBoxFilter));
            }
        }

        private Types? _selectedTypeFilter;
        public Types? SelectedTypeFilter
        {
            get => _selectedTypeFilter;
            set
            {
                _selectedTypeFilter = value;
                OnPropertyChanged(nameof(SelectedTypeFilter));
            }
        }

        private bool _isGreaterSelected;
        public bool IsGreaterSelected
        {
            get => _isGreaterSelected;
            set
            {
                _isGreaterSelected = value;
                OnPropertyChanged(nameof(IsGreaterSelected));
            }
        }

        private bool _isLesserSelected;
        public bool IsLesserSelected
        {
            get => _isLesserSelected;
            set
            {
                _isLesserSelected = value;
                OnPropertyChanged(nameof(IsLesserSelected));
            }
        }
        public string Id { get { return _id; } set { _id = value; OnPropertyChanged(nameof(Id)); } }
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged(nameof(Name)); } }
        public string IpAddress { get { return _ipAddress; } set { _ipAddress = value; OnPropertyChanged(nameof(IpAddress)); } }
        public Types? ServerType { get { return _serverType; } set { _serverType = value; OnPropertyChanged(nameof(ServerType)); } }
        #endregion

        #region ErrorHandling

        private ErrorViewModel _errorViewModel = new ErrorViewModel();
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

        private ObservableCollection<ServerViewModel> _tempCollection;
        public ObservableCollection<ServerViewModel> TempCollection
        {
            get => _tempCollection;
            set
            {
                _tempCollection = value;
                OnPropertyChanged(nameof(TempCollection));
            }
        }

        private  ObservableCollection<ServerViewModel> _listViewCollection;
        public ObservableCollection<ServerViewModel> ListViewCollection { get { return _listViewCollection; } set { _listViewCollection = value; OnPropertyChanged(nameof(ListViewCollection)); } }
        public static ObservableCollection<ServerViewModel> EntityColection { get; set; } = new ObservableCollection<ServerViewModel>();
        public ObservableCollection<Types> TypeCollection { get; set; }     

        private void PopulateTypeCollection()
        {
            TypeCollection = new ObservableCollection<Types>();
            foreach (Types type in Enum.GetValues(typeof(Types)))
            {
                TypeCollection.Add(type);
            }
        }
       


        private bool Validate()
        {          
            bool isValid = true;
            // Id
            if (string.IsNullOrWhiteSpace(Id))
            {
                _errorViewModel.ClearError(nameof(Id));
                _errorViewModel.AddError(nameof(Id), "Id is required.");
                isValid = false;
            }
            else if (!int.TryParse(Id, out int idValue) || idValue <= 0)
            {
                _errorViewModel.ClearError(nameof(Id));
                _errorViewModel.AddError(nameof(Id), "Id must be a positive integer.");
                isValid = false;
            }
            else if(idValue > 99999)
            {
                _errorViewModel.ClearError(nameof(Id));
                _errorViewModel.AddError(nameof(Id), "Id cannot be greater then 99999");
                isValid = false;
            }
            else if (EntityColection.Any(item => item.Id == idValue))
            {
                _errorViewModel.ClearError(nameof(Id));
                _errorViewModel.AddError(nameof(Id), "An object with this Id already exists.");
                isValid = false;
            }
            else
            {
                _errorViewModel.ClearError(nameof(Id));
            }

            // Name
            if (string.IsNullOrWhiteSpace(Name))
            {
                _errorViewModel.ClearError(nameof(Name));
                _errorViewModel.AddError(nameof(Name), "Name is required.");
                isValid = false;
            }
            else
            {
                _errorViewModel.ClearError(nameof(Name));
            }

            // IpAddress
            string ipAddressPattern = @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b";
            if (string.IsNullOrWhiteSpace(IpAddress) || !Regex.IsMatch(IpAddress, ipAddressPattern))
            {
                _errorViewModel.ClearError(nameof(IpAddress));
                _errorViewModel.AddError(nameof(IpAddress), "Invalid IP address format. Example: 192.168.0.1");
                isValid = false;
            }
            else
            {
                _errorViewModel.ClearError(nameof(IpAddress));
            }

            // Type 
            if (ServerType == null) 
            {
                _errorViewModel.ClearError(nameof(ServerType));
                _errorViewModel.AddError(nameof(ServerType), "Server type is required.");
                isValid = false;
            }
            else
            {
                _errorViewModel.ClearError(nameof(ServerType));
            }
            return isValid;
        }
        #region Commands
        public ICommand SubmitCommand => new CommandBase(execute => Submit());
        public ICommand SearchCommand => new CommandBase(execute => ApplyFilters());
        public ICommand ResetCommand => new CommandBase(execute => Reset());
        
        private void Submit()
        {
            if (Validate())
            {
                EntityColection.Add(new ServerViewModel(new Server(int.Parse(Id), Name, IpAddress, ServerType)));
                GlobalVar.IsSaved = false;
                Id = Name = IpAddress = "";
                ServerType = null;
            }
        }
        private void Reset()
        {
            ListViewCollection = TempCollection;
            TempCollection = null; 
        }
        private void ApplyFilters()
        {
            TempCollection = new ObservableCollection<ServerViewModel>(EntityColection); // Backup original collection

            FilteredServers.Clear();

            // Apply filters based on user selections
            var filteredList = EntityColection.Where(server =>
            {
                bool idCondition = true;
                if (!string.IsNullOrEmpty(TextBoxFilter) && int.TryParse(TextBoxFilter, out int filterValue))
                {
                    if (IsGreaterSelected)
                        idCondition = server.Id > filterValue;
                    else if (IsLesserSelected)
                        idCondition = server.Id < filterValue;
                    else
                        idCondition = server.Id == filterValue;
                }

                bool typeCondition = true;
                if (SelectedTypeFilter != null)
                {
                    typeCondition = server.ServerType.Type == SelectedTypeFilter.Value;
                }

                return idCondition && typeCondition;
            });

            foreach (var server in filteredList)
            {
                FilteredServers.Add(server);
            }
            ListViewCollection = FilteredServers;
        }


        #endregion

        public EntitiesViewModel() 
        {
            FilteredServers = new ObservableCollection<ServerViewModel>();
            ListViewCollection =  EntityColection;
            PopulateTypeCollection();
            
            _errorViewModel = new ErrorViewModel();
            _errorViewModel.ErrorsChanged += ErrorViewModel_ErrorsChanged;
        }

    }
}

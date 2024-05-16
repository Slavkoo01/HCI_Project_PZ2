using NetworkService.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NetworkService.ViewModel.Form
{
    public class FilterForm : ViewModelBase, INotifyDataErrorInfo
    {
        private bool _filterFlag = false;
        private string _textBoxFilter;
        private bool _isLesserSelected;
        private bool _isGreaterSelected;
        private bool _isEqualSelected;
        private Types? _selectedTypeFilter;
        private ObservableCollection<ServerViewModel> _filteredServers;

        private string _idFilter;
        public string IdFilter { get { return _idFilter; } set {  _idFilter = value; OnPropertyChanged(nameof(IdFilter)); } }

        private Types? _typeFilter;
        public Types? TypeFilter { get { return _typeFilter; } set { _typeFilter = value; OnPropertyChanged(nameof(TypeFilter)); } }
       
        
        public string TextBoxFilter { get => _textBoxFilter; set { _textBoxFilter = value; OnPropertyChanged(nameof(TextBoxFilter)); } }
        public Types? SelectedTypeFilter { get => _selectedTypeFilter; set { _selectedTypeFilter = value; OnPropertyChanged(nameof(SelectedTypeFilter)); } }
        public bool IsGreaterSelected { get => _isGreaterSelected; set { _isGreaterSelected = value; OnPropertyChanged(nameof(IsGreaterSelected)); } }
        public bool IsLesserSelected { get => _isLesserSelected; set { _isLesserSelected = value; OnPropertyChanged(nameof(IsLesserSelected)); } }
        public bool IsEqualSelected { get => _isEqualSelected; set { _isEqualSelected = value; OnPropertyChanged(nameof(IsEqualSelected)); } }
        public ObservableCollection<ServerViewModel> FilteredServers { get => _filteredServers; set { _filteredServers = value; OnPropertyChanged(nameof(FilteredServers)); } }



        public ObservableCollection<ServerViewModel> Filter()
        {
            _filterFlag = true;
            FilteredServers.Clear();

            var filteredList = EntitiesViewModel.EntityColection.Where(server =>
            {
                bool idCondition = true;
                if (!string.IsNullOrEmpty(TextBoxFilter) && int.TryParse(TextBoxFilter, out int filterValue))
                {
                    if (IsGreaterSelected)
                        idCondition = server.Id > filterValue;
                    else if (IsLesserSelected)
                        idCondition = server.Id < filterValue;
                    else if (IsEqualSelected)
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
            return FilteredServers;
        }

        public ObservableCollection<ServerViewModel> ResetFilter()
        {
            if (_filterFlag)
            {              
                _filterFlag = !_filterFlag;
                return EntitiesViewModel.EntityColection;
            }           
                IsGreaterSelected = IsLesserSelected = IsEqualSelected = false;
                TextBoxFilter = string.Empty;
                SelectedTypeFilter = null;
                _errorViewModel.ClearError(nameof(IdFilter));
                _errorViewModel.ClearError(nameof(TypeFilter));
                return EntitiesViewModel.EntityColection;
        }
        #region ValidationSetup
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

        public bool IsValid()
        {
            bool isValid = true;

            
            // TextBoxFilter
            
            
            // Data  
            if (string.IsNullOrWhiteSpace(TextBoxFilter) && SelectedTypeFilter == null && !(IsGreaterSelected || IsLesserSelected || IsEqualSelected))
            {
                _errorViewModel.ClearError(nameof(IdFilter));
                _errorViewModel.ClearError(nameof(TypeFilter));
                _errorViewModel.AddError(nameof(IdFilter), "Make a Id Filter");
                _errorViewModel.AddError(nameof(TypeFilter), "Make a Type Filter");
                return false;
            }

            if(!int.TryParse(TextBoxFilter, out int filterValue) && SelectedTypeFilter == null)
            {
                _errorViewModel.ClearError(nameof(IdFilter));
                _errorViewModel.ClearError(nameof(TypeFilter));
                _errorViewModel.AddError(nameof(IdFilter), "TextBoxFilter must be a valid integer.");
                isValid = false;
            }
            else if (!(IsGreaterSelected || IsLesserSelected || IsEqualSelected) && SelectedTypeFilter == null)
            {

                _errorViewModel.ClearError(nameof(IdFilter));
                _errorViewModel.ClearError(nameof(TypeFilter));
                _errorViewModel.AddError(nameof(IdFilter), "Select Option: Lesser, Greater or Equal");
                isValid = false;
            }
            else
            {
                _errorViewModel.ClearError(nameof(IdFilter));
            }



            return isValid;
        }
        #endregion
        public FilterForm()
        {
            FilteredServers = new ObservableCollection<ServerViewModel>();
            _errorViewModel = new ErrorViewModel();
            _errorViewModel.ErrorsChanged += ErrorViewModel_ErrorsChanged;
        }
    }
}

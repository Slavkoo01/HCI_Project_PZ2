using NetworkService.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetworkService.ViewModel.Base
{
    public class ServerForm : ViewModelBase, INotifyDataErrorInfo
    {
        private string _id;
        private string _name;
        private string _ipAddress;
        private Types? _serverType;

        public string Id { get { return _id; } set { _id = value; OnPropertyChanged(nameof(Id)); } }
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged(nameof(Name)); } }
        public string IpAddress { get { return _ipAddress; } set { _ipAddress = value; OnPropertyChanged(nameof(IpAddress)); } }
        public Types? ServerType { get { return _serverType; } set { _serverType = value; OnPropertyChanged(nameof(ServerType)); } }
       
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

        public bool Validate()
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
            else if (idValue > 99999)
            {
                _errorViewModel.ClearError(nameof(Id));
                _errorViewModel.AddError(nameof(Id), "Id cannot be greater then 99999");
                isValid = false;
            }
            else if (EntitiesViewModel.EntityColection.Any(item => item.Id == idValue))
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
            else if (Name.Length > 12)
            {
                _errorViewModel.ClearError(nameof(Name));
                _errorViewModel.AddError(nameof(Name), "Name is too long.");
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

        #endregion

        public void ResetValues()
        {
            Id = Name = IpAddress = "";
            ServerType = null;
        }

        public ServerViewModel CreateViewModel()
        {
            return new ServerViewModel(new Server(int.Parse(Id), Name, IpAddress, ServerType));
        }

        public ServerForm()
        {
            _errorViewModel = new ErrorViewModel();
            _errorViewModel.ErrorsChanged += ErrorViewModel_ErrorsChanged;
        }
    }
}

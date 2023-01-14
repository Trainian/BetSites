using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPF.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Settings _settings = new Settings();
        private UserInformation _userInformation = new UserInformation();

        public Settings Settings
        {
            get => _settings;
            set
            {
                if (_settings != value)
                {
                    _settings = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public UserInformation UserInformation 
        {
            get => _userInformation;
            set
            {
                if(_userInformation != value)
                {
                    _userInformation = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

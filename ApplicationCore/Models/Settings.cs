using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public sealed class Settings : INotifyPropertyChanged
    {
        private string _phoneNumber = "+7";
        private string _password = "";
        private int _scrolls = 1;
        private int _betRate = 50;
        private bool _betIsDisabled = false;

        [Phone]
        public string PhoneNumber
        {
            get => _phoneNumber; 
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber= value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Password 
        { 
            get => _password; 
            set
            {
                if (_password != value)
                {
                    _password= value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int Scrolls 
        { 
            get => _scrolls; 
            set
            {
                if (_scrolls != value)
                {
                    _scrolls= value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int BetRate 
        { 
            get => _betRate; 
            set
            {
                if (_betRate != value)
                {
                    _betRate = value;
                    NotifyPropertyChanged();
                }
            } 
        }
        public bool BetIsDisabled 
        { 
            get => _betIsDisabled;
            set
            {
                if (_betIsDisabled != value)
                {
                    _betIsDisabled = value;
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

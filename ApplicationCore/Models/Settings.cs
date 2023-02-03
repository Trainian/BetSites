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
        private bool _betToWinner = false;
        private decimal _minRatio = 0;
        private decimal _maxRatio = 99;
        private DateTime _startDTSimulate = DateTime.Now;
        private DateTime _endDTSimulate = DateTime.Now;
        private string _textDTSimulate = "";

        [Phone]
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
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
                    _password = value;
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
                    _scrolls = value;
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

        public decimal MinRatio
        {
            get => _minRatio;
            set
            {
                if (_minRatio != value)
                {
                    _minRatio = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public decimal MaxRatio
        {
            get => _maxRatio;
            set
            {
                if (_maxRatio != value)
                {
                    _maxRatio = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool BetToWinner 
        { 
            get => _betToWinner;
            set
            {
                if(_betToWinner != value)
                {
                    _betToWinner = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public DateTime StartDTSimulate
        {
            get => _startDTSimulate;
            set
            {
                if (_startDTSimulate != value)
                {
                    _startDTSimulate = value;
                    TextDTSimulate = "Changed";
                    NotifyPropertyChanged();
                }
            }
        }

        public DateTime EndDTSimulate
        {
            get => _endDTSimulate;
            set
            {
                if (_endDTSimulate != value)
                {
                    _endDTSimulate = value;
                    TextDTSimulate = "Changed";
                    NotifyPropertyChanged();
                }
            }
        }

        public string TextDTSimulate
        {
            get => _textDTSimulate;
            set
            {
                if (StartDTSimulate == EndDTSimulate)
                    _textDTSimulate = $"{StartDTSimulate.ToShortDateString()}";
                else
                    _textDTSimulate = $"{StartDTSimulate.ToShortDateString()} - {EndDTSimulate.ToShortDateString()}";

                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

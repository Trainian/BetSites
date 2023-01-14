using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class UserInformation : INotifyPropertyChanged
    {
        private int _balance;
        private int _moneyWon;
        private int _moneyLost;
        private int _betsWin;
        private int _betsLost;
        private int _activeBets;
        private int _allTimeBets;

        public int Balance
        {
            get => _balance;
            set
            {
                if (_balance != value)
                {
                    _balance = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int MoneyWon
        {
            get => _moneyWon;
            set
            {
                if (_moneyWon != value)
                {
                    _moneyWon = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int MoneyLost
        {
            get => _moneyLost;
            set
            {
                if (_moneyLost != value)
                {
                    _moneyLost = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int BetsWin
        {
            get => _betsWin;
            set
            {
                if (_betsWin != value)
                {
                    _betsWin = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int BetsLost 
        { 
            get => _betsLost;
            set
            {
                if(_betsLost != value)
                {
                    _betsLost = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int ActiveBets 
        { 
            get => _activeBets;
            set
            {
                if(_activeBets != value)
                {
                    _activeBets = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int AllTimeBets 
        { 
            get => _allTimeBets;
            set
            {
                if(_allTimeBets != value)
                {
                    _allTimeBets = value;
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

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPF.ViewModels
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private string _name;
        private string _auxiliaryLocator;
        private DateTime createDate;

        public string Name { 
            get { return _name; } 
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            } 
        }

        public string AuxiliaryLocator
        {
            get { return _name; }
            set
            {
                _auxiliaryLocator = value;
                OnPropertyChanged("AuxiliaryLocator");
            }
        }
        public DateTime CreateDate
        {
            get { return createDate; }
            set
            {
                createDate = value;
                OnPropertyChanged("CreateDate");
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

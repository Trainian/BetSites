using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Interfaces
{
    public interface ISettingsService
    {
        public Settings GetSettings();
        public void SetSettings(Settings settings);
    }
}

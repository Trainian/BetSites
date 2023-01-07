using ApplicationCore.Repositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPF.Interfaces;
using WPF.Parsers;
using WPF.Services;
using WPF.Services.Factory;

namespace WPF
{
    public partial class App : Application
    {
        private IServiceProvider serviceProvider;

        public App()
        {
            serviceProvider = ServiceProviderFactory.Get;
        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }

    }
}

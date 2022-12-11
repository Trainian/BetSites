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
using System.Windows.Data;
using WPF.Interfaces;
using WPF.Parsers;
using WPF.Services;

namespace WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ServiceProvider _serviceProvider;
        public App()
        {
            //ServiceCollection services = new ServiceCollection();
            //ConfigureServices(services);
            //_serviceProvider = services.BuildServiceProvider();
            //ServiceProviderFactory.Create();
            _serviceProvider = ServiceProviderFactory.Get;
        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }

        //private void ConfigureServices(ServiceCollection services)
        //{
        //    services.AddDbContext<BetContext>(options =>
        //    {
        //        options.UseSqlite("Data Source=BetDB.db").UseLazyLoadingProxies();
        //    });
        //    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        //    services.AddScoped(typeof(IBetRepository), typeof(BetRepository));
        //    services.AddScoped(typeof(ICoefficientRepository), typeof(CoefficientRepository));
        //    services.AddScoped(typeof(IBetService), typeof(BetService));
        //    services.AddSingleton<MainWindow>();
        //}
    }
}

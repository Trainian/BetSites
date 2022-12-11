﻿using ApplicationCore.Repositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Interfaces;
using WPF.Parsers;

namespace WPF.Services
{
    public static class ServiceProviderFactory
    {
        private static ServiceProvider? _serviceProvider;

        public static ServiceProvider Get => _serviceProvider ??= Create();

        private static ServiceProvider Create()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            return services.BuildServiceProvider();
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddDbContext<BetContext>(options =>
            {
                options.UseSqlite("Data Source=BetDB.db;Cache=Shared").UseLazyLoadingProxies().EnableServiceProviderCaching();
            });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IBetRepository), typeof(BetRepository));
            services.AddScoped(typeof(ICoefficientRepository), typeof(CoefficientRepository));
            services.AddScoped(typeof(IBetService), typeof(BetService));
            services.AddScoped(typeof(IStatisticService), typeof(StatisticService));

            services.AddSingleton<MainWindow>();
        }
    }
}

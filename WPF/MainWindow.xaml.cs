using ApplicationCore.Models;
using ApplicationCore.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WPF.Interfaces;
using WPF.Parsers;
using WPF.Services;
using WPF.Static;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BetContext _context;
        private CollectionViewSource BetsViewSource;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private ObservableCollection<Bet> Bets = new ObservableCollection<Bet>();
        private CollectionViewSource CoefficientsViewSource;
        private ObservableCollection<Coefficient> Coefficients = new ObservableCollection<Coefficient>();
        private IStatisticService _serviceStatistic;
        public static object locker = new object();

        public MainWindow(BetContext context)
        {
            InitializeComponent();
            _context = context;
            BetsViewSource = (CollectionViewSource)FindResource(nameof(BetsViewSource));
            CoefficientsViewSource = (CollectionViewSource)FindResource(nameof(CoefficientsViewSource));
            var services = ServiceProviderFactory.Get;
            _serviceStatistic = services.GetService<IStatisticService>()!;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _context.Bets.Load();
            _context.Coefficients.Load();

            Bets = _context.Bets.Local.ToObservableCollection();
            Bets.CollectionChanged += Bets_CollectionChanged;
            BindingOperations.EnableCollectionSynchronization(Bets, locker);

            Coefficients = _context.Coefficients.Local.ToObservableCollection();
            Coefficients.CollectionChanged += Coefficients_CollectionChanged;
            BindingOperations.EnableCollectionSynchronization(Coefficients, locker);

            BetsViewSource.Source = Bets;
        }

        private void Coefficients_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() => coefficientDataGrid.Items.Refresh());

            ObservableCollection<Coefficient> obsSender = sender as ObservableCollection<Coefficient>;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    App.Current.Dispatcher.Invoke(() => betDataGrid.Items.Refresh());
                    break;

                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Remove:
                    break;

                case NotifyCollectionChangedAction.Replace:
                    break;

                case NotifyCollectionChangedAction.Reset:
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void Bets_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() => betDataGrid.Items.Refresh());
        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            int scrolls;
            var isCount = int.TryParse(Scrolls.Text, out scrolls);

            if(isCount != true)
            {
                MessageBox.Show("Count scrolls most be count");
                return;
            }

            try
            {
                //await Task.Run(async () => await FonBetStart(scrolls),_cancellationTokenSource.Token).ConfigureAwait(false);
                await FonBetStart(scrolls);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка интернет соединения.\n{ex.Message}");
            }
        }


        protected override async void OnClosing(CancelEventArgs e)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _context.Dispose();
            base.OnClosing(e);
        }
        private async Task FonBetStart(int scrolls)
        {
            await Task.Run(() => new FonBetParser().Run(_cancellationTokenSource.Token, scrolls)).ConfigureAwait(false);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }

        private async void CalculateStatistic_Click(object sender, RoutedEventArgs e)
        {
            Win_Changed.Content = await _serviceStatistic.GetPercentChangedWiners().ConfigureAwait(false);
            Win_Changed.Content += "%";

            Win_Favorits.Content = await _serviceStatistic.GetPercentWinsFavorits().ConfigureAwait(false);
            Win_Favorits.Content += "%";

            Win_Percent.Content = await _serviceStatistic.GetPercentWins().ConfigureAwait(false);
            Win_Percent.Content += "%";
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp1.Models;

namespace WpfApp1.ViewModels
{
    public class ConstructionViewModel : INotifyPropertyChanged
    {
        private Random _random = new Random();
        private ObservableCollection<House> _houses;
        private string _statusMessage;

        public ObservableCollection<House> Houses
        {
            get => _houses;
            set
            {
                _houses = value;
                OnPropertyChanged(nameof(Houses));
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }

        public ICommand StartConstructionCommand { get; }

        public ConstructionViewModel()
        {
            Houses = new ObservableCollection<House>();
            StartConstructionCommand = new RelayCommand(async () => await StartConstructionAsync());
        }

        public async Task StartConstructionAsync()
        {
            var house = new House();
            house.MaterialsDepleted += OnMaterialsDepleted;
            house.ReadyForRoofing += OnReadyForRoofing;
            Houses.Add(house);

            StatusMessage = "Laying foundation...";
            await Task.Run(() => house.LayFoundation());

            if (_random.NextDouble() > 0.1) // Simulate 10% chance of materials depletion
            {
                StatusMessage = "Building walls...";
                await Task.Run(() => house.BuildWalls());

                StatusMessage = "Covering roof...";
                await Task.Run(() => house.CoverRoof());

                StatusMessage = "House construction completed!";
            }
            else
            {
                StatusMessage = "Materials depleted!";
            }
        }

        private void OnMaterialsDepleted(object sender, EventArgs e)
        {
            StatusMessage = "Materials depleted, please deliver more materials.";
        }

        private void OnReadyForRoofing(object sender, EventArgs e)
        {
            StatusMessage = "Walls built, ready for roofing.";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();
        public async void Execute(object parameter) => await _execute();
        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}

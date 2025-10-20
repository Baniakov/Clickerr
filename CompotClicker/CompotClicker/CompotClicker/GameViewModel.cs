using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using Xamarin.Forms;
using System.Windows.Input;
using Xamarin.Essentials;


namespace CompotClicker
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private readonly GameModel _model;

        private void SaveGame()
        {
            Preferences.Set(nameof(_model.TotalPoints), _model.TotalPoints);
            Preferences.Set(nameof(_model.PointsPerClick), _model.PointsPerClick);
            Preferences.Set(nameof(_model.PointsPerSecond), _model.PointsPerSecond);
            Preferences.Set(nameof(_model.ClickCount), _model.ClickCount);
        }

        private void LoadGame()
        {
            _model.TotalPoints = Preferences.Get(nameof(_model.TotalPoints), 0.0);
            _model.PointsPerClick = Preferences.Get(nameof(_model.PointsPerClick), 1.0);
            _model.PointsPerSecond = Preferences.Get(nameof(_model.PointsPerSecond), 0.0);
            _model.ClickCount = Preferences.Get(nameof(_model.ClickCount), 0);
        }


        private readonly Timer _timer;

        public event PropertyChangedEventHandler PropertyChanged;

        public double TotalPoints => _model.TotalPoints;
        public double PointsPerClick => _model.PointsPerClick;
        public int ClickCount => _model.ClickCount;

        public ICommand ClickCommand { get; }
        public ICommand UpgradeClickCommand { get; }

        private double _upgradeCost = 50;

        public GameViewModel()
        {
            _model = new GameModel();
            LoadGame();
            ClickCommand = new Command(OnClick);

            _timer = new Timer(1000);
            _timer.Elapsed += (s, e) => OnTimerTick();
            _timer.Start();
        }

        private void OnClick()
        {
            _model.AddClick();
            OnPropertyChanged(nameof(TotalPoints));
            OnPropertyChanged(nameof(ClickCount));
            SaveGame();
        }

        private void OnTimerTick()
        {
            _model.AddPassivePoints(1);
            OnPropertyChanged(nameof(TotalPoints));
        }

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            Device.BeginInvokeOnMainThread(() =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name))
            );
        }

    }
}

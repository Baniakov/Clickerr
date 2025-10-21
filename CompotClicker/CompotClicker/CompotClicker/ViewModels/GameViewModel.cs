using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using Xamarin.Forms;
using System.Windows.Input;

namespace CompotClicker
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private readonly GameService _game;
        private readonly Timer _timer;

        public event PropertyChangedEventHandler PropertyChanged;

        public double TotalPoints => _game.TotalPoints;
        public double PointsPerClick => _game.PointsPerClick;
        public int ClickCount => _game.ClickCount;

        public ICommand ClickCommand { get; }

        public GameViewModel()
        {
            _game = GameService.Instance;
            ClickCommand = new Command(OnClick);

            MessagingCenter.Subscribe<ShopViewModel>(this, "UpgradePurchased", (sender) =>
            {
                OnPropertyChanged(nameof(PointsPerClick));
                OnPropertyChanged(nameof(TotalPoints));
            });
        }


        private void OnClick()
        {
            _game.AddClick();
            OnPropertyChanged(nameof(TotalPoints));
            OnPropertyChanged(nameof(ClickCount));
        }

        private void OnTimerTick()
        {
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

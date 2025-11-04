using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using System.Windows.Input;

namespace CompotClicker
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private readonly GameService _game;

        public event PropertyChangedEventHandler PropertyChanged;

        // Śledzimy poprzedni level żeby wykryć awans i wyświetlić alert
        private int _previousLevel;

        public double TotalPoints => _game.TotalPoints;
        public double PointsPerClick => _game.PointsPerClick;
        public int ClickCount => _game.ClickCount;

        // Postęp względem levela
        public double Progress => _game.ExperienceCurrent / _game.XPToNextLevel;

        public string LevelDisplay => $"Poziom {_game.Level} — {_game.ExperienceCurrent:F0}/{_game.XPToNextLevel:F0} XP";

        public ICommand ClickCommand { get; }

        public GameViewModel()
        {
            _game = GameService.Instance;
            ClickCommand = new Command(OnClick);

            _previousLevel = _game.Level;

            // Subskrybuj zmiany stanu gry (np. przy zdobyciu XP/poziomie)
            _game.GameStateChanged += OnGameStateChanged;

            // Aby MessageCenter / inne ViewModel-y mogły powiadamiać
            MessagingCenter.Subscribe<ShopViewModel>(this, "UpgradePurchased", (sender) =>
            {
                NotifyAll();
            });
        }

        private void OnClick()
        {
            _game.AddClick();
            NotifyAll();
        }

        private void OnGameStateChanged()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                // Sprawdź, czy level wzrósł — jeśli tak, pokaż alert
                if (_game.Level > _previousLevel)
                {
                    int gained = _game.Level - _previousLevel;
                    Application.Current.MainPage.DisplayAlert("Awans!", $"Gratulacje — osiągnąłeś poziom {_game.Level}!\nOdblokowano nowe możliwości.", "OK");
                    _previousLevel = _game.Level;
                }

                // Zaktualizuj bindingi
                NotifyAll();
            });
        }

        private void NotifyAll()
        {
            OnPropertyChanged(nameof(TotalPoints));
            OnPropertyChanged(nameof(PointsPerClick));
            OnPropertyChanged(nameof(ClickCount));
            OnPropertyChanged(nameof(Progress));
            OnPropertyChanged(nameof(LevelDisplay));
        }

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            Device.BeginInvokeOnMainThread(() =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name))
            );
        }
    }
}

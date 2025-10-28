using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;

namespace CompotClicker
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private GameService _gameService;

        public event PropertyChangedEventHandler PropertyChanged;

        public double PointsPerClick => _gameService.PointsPerClick;

        public ICommand ResetCommand { get; }
        public ICommand ChangeThemeCommand { get; }

        public SettingsViewModel()
        {
            _gameService = GameService.Instance;

            // Inicjalizacja komend
            ResetCommand = new Command(OnReset);
            ChangeThemeCommand = new Command(OnChangeTheme);
        }

        private void OnReset()
        {
            _gameService.ResetGame(); // Wywołanie metody resetującej
            OnPropertyChanged(nameof(PointsPerClick));
            Application.Current.MainPage.DisplayAlert("Sukces", "Statystyki zostały zresetowane!", "OK");
        }

        private void OnChangeTheme()
        {
            // Sprawdzamy, który motyw jest aktualnie załadowany
            var currentTheme = Application.Current.Resources.MergedDictionaries
                                 .FirstOrDefault(d => d.ContainsKey("BackgroundColor")) as ResourceDictionary;

            // Jeśli aktualny motyw to jasny, przełączamy na ciemny
            if (currentTheme == null || currentTheme.ContainsKey("BackgroundColor") &&
                currentTheme["BackgroundColor"].ToString() == "White")
            {
                // Usuń wszystkie poprzednie zasoby
                Application.Current.Resources.MergedDictionaries.Clear();

                // Dodaj motyw ciemny do MergedDictionaries
                var darkTheme = new ResourceDictionary();
                darkTheme.Add("BackgroundColor", Color.FromHex("#121212"));
                darkTheme.Add("TextColor", Color.White);
                darkTheme.Add("ButtonBackgroundColor", Color.FromHex("#6200EE"));
                darkTheme.Add("ButtonTextColor", Color.White);
                Application.Current.Resources.MergedDictionaries.Add(darkTheme);
            }
            else
            {
                // Usuń wszystkie poprzednie zasoby
                Application.Current.Resources.MergedDictionaries.Clear();

                // Dodaj motyw jasny do MergedDictionaries
                var lightTheme = new ResourceDictionary();
                lightTheme.Add("BackgroundColor", Color.White);
                lightTheme.Add("TextColor", Color.Black);
                lightTheme.Add("ButtonBackgroundColor", Color.FromHex("#4CAF50"));
                lightTheme.Add("ButtonTextColor", Color.White);
                Application.Current.Resources.MergedDictionaries.Add(lightTheme);
            }

            // Wyświetlanie powiadomienia o zmianie motywu
            string newTheme = currentTheme == null || currentTheme["BackgroundColor"].ToString() == "White" ? "ciemny" : "jasny";
            Application.Current.MainPage.DisplayAlert("Zmiana motywu", $"Przełączono na motyw {newTheme}.", "OK");
        }





        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

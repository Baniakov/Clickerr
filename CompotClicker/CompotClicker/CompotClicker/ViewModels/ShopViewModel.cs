using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace CompotClicker
{
    public class ShopViewModel : INotifyPropertyChanged
    {

        private GameService _game;
        public event PropertyChangedEventHandler PropertyChanged;

        public double TotalPoints
        {
            get { return _game.TotalPoints; }
        }

        public ObservableCollection<UpgradeItem> Upgrades { get; private set; }

        public ShopViewModel()
        {
            _game = GameService.Instance;

            Upgrades = new ObservableCollection<UpgradeItem>
            {
                new UpgradeItem("upgrade_1", "Większa łyżka", 50, 1),
                new UpgradeItem("upgrade_2", "Złoty garnek", 200, 5),
                new UpgradeItem("upgrade_3", "Magiczny owoc", 1000, 25)
            };


            // Oznacz już zakupione
            foreach (var u in Upgrades)
            {
                if (_game.PurchasedItems.Contains(u.Id))
                    u.IsPurchased = true;
            }

            // Odśwież etykietę punktów co sekundę
            Device.StartTimer(System.TimeSpan.FromSeconds(1), () =>
            {
                OnPropertyChanged("TotalPoints");
                return true;
            });


        }

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }
    }
}
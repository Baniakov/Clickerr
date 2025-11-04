using Xamarin.Forms;

namespace CompotClicker
{
    public class UpgradeItem
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public double Cost { get; private set; }               // koszt w XP (waluta)
        public double BonusPerClick { get; private set; }
        public bool IsPurchased { get; set; }

        public Command BuyCommand { get; private set; }

        public UpgradeItem(string id, string name, double cost, double bonus)
        {
            Id = id;
            Name = name;
            Cost = cost;
            BonusPerClick = bonus;
            IsPurchased = false;

            BuyCommand = new Command(OnBuy);
        }

        private void OnBuy()
        {
            if (GameService.Instance.TryPurchaseXP(Cost, BonusPerClick))
            {
                IsPurchased = true;
                Application.Current.MainPage.DisplayAlert("Sukces", "Zakupiono: " + Name + "!", "OK");
                GameService.Instance.MarkItemAsPurchased(Id);

                MessagingCenter.Send(this, "UpgradePurchased");
            }


            // Używamy TryPurchaseXP — zakup opłacany jest z dostępnego XP (waluty).
            if (GameService.Instance.TryPurchaseXP(Cost, BonusPerClick))
            {
                IsPurchased = true;
                Application.Current.MainPage.DisplayAlert("Sukces", "Zakupiono: " + Name + "!", "OK");
                GameService.Instance.MarkItemAsPurchased(Id);
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Brak XP", "Nie stać cię na " + Name + ".", "OK");
            }
        }
    }
}

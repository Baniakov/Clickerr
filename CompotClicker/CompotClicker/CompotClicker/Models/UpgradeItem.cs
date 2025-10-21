using Xamarin.Forms;

namespace CompotClicker
{
    public class UpgradeItem
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public double Cost { get; private set; }
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
            if (IsPurchased)
            {
                Application.Current.MainPage.DisplayAlert("Informacja", Name + " już zakupiono.", "OK");
                return;
            }

            if (GameService.Instance.TryPurchase(Cost, BonusPerClick))
            {
                IsPurchased = true;
                Application.Current.MainPage.DisplayAlert("Sukces", "Zakupiono: " + Name + "!", "OK");
                GameService.Instance.MarkItemAsPurchased(Id);
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Brak punktów", "Nie stać cię na " + Name + ".", "OK");
            }
        }
    }
}

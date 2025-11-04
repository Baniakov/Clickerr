using CompotClicker.Views;
using Xamarin.Forms;

namespace CompotClicker
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Define the TabbedPage navigation
            MainPage = new TabbedPage
            {
                Children =
                {
                    new GamePage { Title = "Kompot" }, // Tab 1 for GamePage
                    new ShopPage { Title = "Sklep" },  // Tab 2 for ShopPage
                    new SettingsPage() { Title = "Ustawienia" } // Tab 3 for SettingsPage
                }
            };
        }
    }
}
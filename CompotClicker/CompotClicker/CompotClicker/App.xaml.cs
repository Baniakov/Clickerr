using Xamarin.Forms;

namespace CompotClicker
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new TabbedPage
            {
                Children =
                {
                    new GamePage { Title = "Kompot" },
                    new ShopPage { Title = "Sklep" },
                    new ContentPage
                    {
                        Title = "Ustawienia",
                        Content = new Label
                            {
                                Text = "Tu możesz zmienić ustawienia.",
                                HorizontalOptions = LayoutOptions.Center,
                                VerticalOptions = LayoutOptions.Center
                            }
                        }
                    }
            };

        }
    }
}
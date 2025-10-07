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

                    new ContentPage
                    {
                        Title = "Ustawienia",
                        Content = new Label
                        {
                            Text = "Tu możesz zmienić ustawienia.",
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center
                        }
                    },
                    new ContentPage
                    {
                        Title = "Kompot",
                        Content = new Label
                        {
                            Text = "Witaj w CompotClicker!",
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center
                        }
                    },
                    new ContentPage
                    {
                        Title = "Sklep",
                        Content = new Label
                        {
                            Text = "Tu możesz przewalić swój dorobek życia.",
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center
                        }
                }
            }
            };
    }
    } }

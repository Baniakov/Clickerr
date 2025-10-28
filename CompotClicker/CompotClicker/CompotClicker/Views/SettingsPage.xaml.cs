using CompotClicker.Views;
using CompotClicker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompotClicker.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = new SettingsViewModel(); // Powiązanie ViewModelu
        }

    }
}

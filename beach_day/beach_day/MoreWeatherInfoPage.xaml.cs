using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beach_day.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace beach_day
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MoreWeatherInfoPage : ContentPage
	{
		public MoreWeatherInfoPage ()
		{
			InitializeComponent ();
		}

        public MoreWeatherInfoPage(WeatherDayData weatherData)
        {
            InitializeComponent();

            BindingContext = weatherData;
        }

        async void Button_Clicked(object sender, EventArgs e)
        {
            //neither work
            //Navigation.PopModalAsync(); 
            await Navigation.PopAsync();
        }
    }
}
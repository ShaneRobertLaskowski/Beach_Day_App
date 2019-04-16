using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using beach_day.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;

namespace beach_day
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BeachSelectPage : ContentPage
	{
		public BeachSelectPage ()
		{
			InitializeComponent ();
		}

        async void Beach_SelectionButton_Clicked(object sender, EventArgs e)
        {
            //https://api.darksky.net/forecast/5ee647fd1cf5f4f33a5ee6ecbcd904db/37.8267,-122.4233
            var client = new HttpClient();
            var lat = "37.8267";
            var lng = "-122.4233";
            var weatherRequestURL = "https://api.darksky.net/forecast/5ee647fd1cf5f4f33a5ee6ecbcd904db/" + lat + "," + lng;
            var uri = new Uri(weatherRequestURL);
            DarkSkyForecast weatherForcastData = new DarkSkyForecast();

            var response = await client.GetAsync(uri);
            if(!response.IsSuccessStatusCode)
            {
                await DisplayAlert("Error", "Could Not get weather data :(", "OK");
            }
            else {
                var jsonWeatherContent = await response.Content.ReadAsStringAsync();
                weatherForcastData = JsonConvert.DeserializeObject<DarkSkyForecast>(jsonWeatherContent); //weatherForcastData is now a C# object containg our weather data
                                                                                                         //now take out the Daily (8 days) of data out of this single object and put these 7 objects into a list and then into an observable collection
                ObservableCollection<DailyDatum> weatherCollection = new ObservableCollection<DailyDatum>(weatherForcastData.Daily.Data);
                WeatherList.ItemsSource = weatherCollection; //assigns the observable collection to the listview
            }

        }
    }
}
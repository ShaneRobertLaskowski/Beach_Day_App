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

                List<DailyDatum> rawDailyWeatherData = weatherForcastData.Daily.Data;

                List<WeatherDayData> salientDailyWeatherList = DailyDatumToWeatherDayDataExtraction(rawDailyWeatherData);
                //ObservableCollection<DailyDatum> weatherCollection = new ObservableCollection<DailyDatum>(weatherForcastData.Daily.Data);
                //WeatherList.ItemsSource = weatherCollection; //assigns the observable collection to the listview

                ObservableCollection<WeatherDayData> weatherCollection = new ObservableCollection<WeatherDayData>(salientDailyWeatherList);
                WeatherList.ItemsSource = weatherCollection; //assigns the observable collection to the listview

            }

        }
        List<WeatherDayData> DailyDatumToWeatherDayDataExtraction(List<DailyDatum> rawWeatherData)
        {
            List<WeatherDayData> weatherList = new List<WeatherDayData>();

            long unixTimeDay;
            long unixTimeTempHigh;
            long unixTimeTempLow;
            string day;
            string month;
            string highTempTime;
            string lowTempTime;
            foreach (var rawDayData in rawWeatherData)
            {
                unixTimeDay = rawDayData.Time;
                System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
                dateTime = dateTime.AddSeconds(unixTimeDay).ToLocalTime();
                day = dateTime.ToString("ddd");
                month = dateTime.ToString("MMM");

                unixTimeTempHigh = rawDayData.TemperatureHighTime;
                dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0); //might want to just change the current value of the dateTime variable
                dateTime = dateTime.AddSeconds(unixTimeTempHigh).ToLocalTime(); //go from UTC to client's timezone (PST)
                highTempTime = dateTime.ToString("h:mm tt");

                unixTimeTempLow = rawDayData.TemperatureLowTime;
                dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0); //might want to just change the current value of the dateTime variable
                dateTime = dateTime.AddSeconds(unixTimeTempLow).ToLocalTime(); //go from UTC to client's timezone (PST)
                lowTempTime = dateTime.ToString("h:mm tt");

                weatherList.Add(new WeatherDayData { Day = day, Month = month, TemperatureHigh = rawDayData.TemperatureHigh,
                    TemperatureHighTime = highTempTime, TemperatureLow = rawDayData.TemperatureLow, TemperatureLowTime = lowTempTime, 
                    WindSpeed = rawDayData.WindSpeed, UvIndex = rawDayData.UvIndex, Summary = rawDayData.Summary });

            }

            return weatherList;
        }

        //binds a list of BeachPlace objects to the Picker
        private void ContentPage_Appearing(object sender, EventArgs e)
        {

            BeachPlace beach1 = new BeachPlace { Name = "North Carlsbad Beach", Latitude = 33.163404, Longitude = -117.358215 };
            BeachPlace beach2 = new BeachPlace { Name = "Oceanside Beach", Latitude = 33.194088, Longitude = -117.384206 };
            BeachPlace beach3 = new BeachPlace { Name = "Moonlight Beach", Latitude = 33.047734, Longitude = -117.297918 };
            BeachPlace beach4 = new BeachPlace { Name = "Swami's Beach", Latitude = 33.034890, Longitude = -117.292385 };
            BeachPlace beach5 = new BeachPlace { Name = "San Elijo Beach", Latitude = 33.021370, Longitude = -117.284242 };
            BeachPlace beach6 = new BeachPlace { Name = "Coronado Beach", Latitude = 32.686282, Longitude = -117.191942 };
            BeachPlace beach7 = new BeachPlace { Name = "Ocean Beach", Latitude = 32.750567, Longitude = -117.252592 };


            List<BeachPlace> beaches = new List<BeachPlace>
            {
                beach1, beach2, beach3, beach4, beach5, beach6, beach7
            };

            
            BeachPicker.ItemsSource = new ObservableCollection<BeachPlace>(beaches);

        }
    }
}
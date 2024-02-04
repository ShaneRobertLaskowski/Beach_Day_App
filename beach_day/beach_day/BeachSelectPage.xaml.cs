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
using Xamarin.Essentials; 

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace beach_day
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BeachSelectPage : ContentPage
	{

        ObservableCollection<BeachPlace> beachCollection;

		public BeachSelectPage ()
		{
			InitializeComponent ();
            PopulateBeachListView();

        }

        async void Beach_Picked(object sender, EventArgs e)
        {
            //quick and dirty fix... To do: don't invoke this with function on the Appearing event, call it after InitializeComponent()
            if ((BeachPlace)(((Picker)sender).SelectedItem) == null)
                return;
            QueryingWeatherIndicator.IsRunning = true;

            var beachSelected = (BeachPlace)(((Picker)sender).SelectedItem);
            var lat = beachSelected.Latitude.ToString();
            var lng = beachSelected.Longitude.ToString();

            var client = new HttpClient();
            var weatherRequestURL = "https://api.pirateweather.net/forecast/YOUR-PIRATEWEATHER-API-KEY/" + lat + "," + lng;
            //
            // example dark sky api: "https://api.darksky.net/forecast/5ee647fd1cf5f4f33a5ee6ecbcd904db/"
            var uri = new Uri(weatherRequestURL);
            DarkSkyForecast weatherForcastData = new DarkSkyForecast();

            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync(uri); //Wrap custom failure reporting (try/catch + analytics) around this
            }
            catch(Exception networkException)
            {
                Crashes.TrackError(networkException);
                QueryingWeatherIndicator.IsRunning = false; //id use this in a finally, but this should occur after the code blocks below
                await DisplayAlert("Error","Failed to retrieve Weather Data: " + networkException.Message, "OK");
                return;
            }

            if(!response.IsSuccessStatusCode)
            {
                await DisplayAlert("Error", "Could Not get weather data :(", "OK"); 
            }
            else {
                //***note: the API could send us an error JSON (failure on their part)
                Analytics.TrackEvent("DarkSky API called");
                BeachSelectHeaderLabel.Text = beachSelected.Name;  //changes the header of page to name of beach selected

                TodaySectionHeaderLabel.IsVisible = true;  //displays the "Today" label
                DarkSkyLogoMarker.IsVisible = true; //displays the Dark Sky 'watermark' image

                var jsonWeatherContent = await response.Content.ReadAsStringAsync();
                weatherForcastData = JsonConvert.DeserializeObject<DarkSkyForecast>(jsonWeatherContent); //weatherForcastData is now a C# object containg our weather data

                List<DailyDatum> rawDailyWeatherData = weatherForcastData.Daily.Data;  //grabs next 7 days' + today's weather data and stores in a List
                List<WeatherDayData> salientDailyWeatherList = DailyDatumToWeatherDayDataExtraction(rawDailyWeatherData);  //filter out the salient data and put into new list of type WeatherDayData

                //need to remove the 1st List item (if list is not empty) and place its content in the "Today" labelling section of the .xaml
                TodayLabelSection.BindingContext = salientDailyWeatherList[0];
                salientDailyWeatherList.RemoveAt(0);

                ObservableCollection<WeatherDayData> weatherCollection = new ObservableCollection<WeatherDayData>(salientDailyWeatherList);
                WeatherList.ItemsSource = weatherCollection; //assigns the observable collection to the listview

            }
            QueryingWeatherIndicator.IsRunning = false;
        }
        List<WeatherDayData> DailyDatumToWeatherDayDataExtraction(List<DailyDatum> rawWeatherData)
        {
            List<WeatherDayData> weatherList = new List<WeatherDayData>();

            long unixTimeDay;
            long unixTimeTempHigh;
            long unixTimeTempLow;
            string day;
            string month;
            string formattedDate;
            string highTempTime;
            string lowTempTime;
            foreach (var rawDayData in rawWeatherData)
            {
                unixTimeDay = rawDayData.Time;
                System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
                dateTime = dateTime.AddSeconds(unixTimeDay).ToLocalTime();
                day = dateTime.ToString("ddd");
                month = dateTime.ToString("MMM");
                formattedDate = dateTime.ToString("dddd, MMMM dd");

                unixTimeTempHigh = rawDayData.TemperatureHighTime;
                dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0); //might want to just change the current value of the dateTime variable
                dateTime = dateTime.AddSeconds(unixTimeTempHigh).ToLocalTime(); //go from UTC to client's timezone (PST)
                highTempTime = dateTime.ToString("h:mm tt");

                unixTimeTempLow = rawDayData.TemperatureLowTime;
                dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0); //might want to just change the current value of the dateTime variable
                dateTime = dateTime.AddSeconds(unixTimeTempLow).ToLocalTime(); //go from UTC to client's timezone (PST)
                lowTempTime = dateTime.ToString("h:mm tt");

                weatherList.Add(new WeatherDayData { Day = day, Month = month, DateFormatted = formattedDate, TemperatureHigh = rawDayData.TemperatureHigh,
                    TemperatureHighTime = highTempTime, TemperatureLow = rawDayData.TemperatureLow, TemperatureLowTime = lowTempTime, 
                    WindSpeed = rawDayData.WindSpeed, UvIndex = rawDayData.UvIndex, Summary = rawDayData.Summary });

            }

            return weatherList;
        }


        //***instead of just hardcoding, try loading in the Table of BeachPlaces records (if any) 
        //and inserting in the Obs collection (to test, add along with the hardcoded beaches)
        private async void PopulateBeachListView()
        {
            ///
            var allBeaches = await App.BeachPlaceDatabaseInstance.GetAllBeachPlaces();
            beachCollection = new ObservableCollection<BeachPlace>(allBeaches);
            BeachPicker.ItemsSource = beachCollection;

            ///
            
            /*
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

            beachCollection = new ObservableCollection<BeachPlace>(beaches);
            BeachPicker.ItemsSource = beachCollection;
            */
        }

        async void MenuItem_MoreInfo_Clicked(object sender, EventArgs e)
        {
            var menuItem = (MenuItem)sender;
            var weatherDataSet = (WeatherDayData)menuItem.CommandParameter;
            await Navigation.PushAsync(new MoreWeatherInfoPage(weatherDataSet));
        }

        public async void DisplayDirections(object sender, EventArgs e)
        {

            if ((BeachPlace)BeachPicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "You did not select a beach", "OK");
                return;
            }
            BeachPlace userSelectedBeach = (BeachPlace)BeachPicker.SelectedItem;

            //grab the beachplace from picker, if null return, else pass it into the LaunchGoogleMapsDirections(...)
            await LaunchGoogleMapsDirections(userSelectedBeach);
        }

        public async Task LaunchGoogleMapsDirections(BeachPlace userSelectedBeach)
        {

            var location = new Location(userSelectedBeach.Latitude, userSelectedBeach.Longitude);
            var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving, Name = userSelectedBeach.Name };

            await Map.OpenAsync(location, options);
        }

        private async void Toolbar_Beaches_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new Beaches()); 
            await Navigation.PushAsync(new Beaches(beachCollection)); 
            
        }
    }
}
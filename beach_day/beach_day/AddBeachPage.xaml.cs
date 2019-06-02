using beach_day.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.GoogleMaps; //using Xamarin.Forms.Maps;

namespace beach_day
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddBeachPage : ContentPage
	{
        private static ObservableCollection<BeachPlace> beachCollection; //the user will be able to add to this within the Add_Beach_Button_Clicked beach method
        private Map map;

        public AddBeachPage ()
		{
			InitializeComponent ();
		}

        public AddBeachPage(ObservableCollection<BeachPlace> beachPlaces)
        {
            beachCollection = beachPlaces;
            InitializeComponent();
            AddGoogleMap();
        }

        /// <summary>
        ///     creates a google map and assigns it to the appopriate grid location of the Page.  the initial zoom level is set to 75 miles
        ///     and the intial center of the map is focused at San Diego (Lat: 32.7157, Lng: -117.1611).
        /// </summary>
        private void AddGoogleMap()
        {
            map = new Map();
            map.MyLocationEnabled = true;
            map.UiSettings.MyLocationButtonEnabled = true;
            map.MapType = MapType.Hybrid;
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(32.7157, -117.1611), Distance.FromMiles(75.0)));
            /*
            map = new Map(MapSpan.FromCenterAndRadius(new Position(32.7157, -117.1611), Distance.FromMiles(75.0)))
            {
                IsShowingUser = true,
                MapType = MapType.Hybrid,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            */
            map.MapClicked += Map_MapClicked;

            PageGridLayout.Children.Add(map, 0, 0);
            Grid.SetColumnSpan(map, 3);
        }

        /// <summary>
        ///     This method is called when the user taps on the google map.  A pin is placed on the posistion
        ///     of the tap and the lat/lng Entry views are filled out with the apporpriate values.
        /// </summary>
        /// <param name="sender">The google map</param>
        /// <param name="e"></param>
        private void Map_MapClicked(object sender, MapClickedEventArgs e)
        {
            double roundedLat = Math.Round(e.Point.Latitude, 6);
            double roundedLng = Math.Round(e.Point.Longitude, 6);

            BeachEntryLat.Text = roundedLat.ToString();
            BeachEntryLng.Text = roundedLng.ToString();

            Map mapClickedOn = (Map)sender;

            Pin pinToAdd = new Pin
            {
                Position = new Position(e.Point.Latitude, e.Point.Longitude),
                Label = "Geocode location here",
                Address = roundedLat.ToString() + ", " + roundedLng.ToString()
            };


            mapClickedOn.Pins.Clear();
            mapClickedOn.Pins.Add(pinToAdd);
        }


        /// <summary>
        ///     once the 3 Entry view are filled in (the name, lat, and lng entries) the user input is checked, and if everything is appopriate, then the beach
        ///     info is placed into a new BeachPlace object that is added to the global observable collection.  ***will need to add the BeachPlace to a Database Table 
        ///     for persistant storage and for the re-creation of a BeachSelectPage instance.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Add_Beach_Button_Clicked(object sender, EventArgs e)
        {
            //add beach to observable collection and to the DB
            
            if (CheckProperLatitudeValue(BeachEntryLat.Text) && CheckProperLongitudeValue(BeachEntryLng.Text) && CheckProperBeachName(BeachEntryName.Text))
            {
                string name = BeachEntryName.Text;
                //may want to just pass the lat/lng variables into CheckProper methods by reference, that way the parse method doesn't need to be called again for each one
                double lat = double.Parse(BeachEntryLat.Text);
                double lng = double.Parse(BeachEntryLng.Text);
                BeachPlace newBeachToAdd = new BeachPlace { Name = name, Latitude = lat, Longitude = lng };

                beachCollection.Add(newBeachToAdd);

                //add the newBeachToAdd to the DB
                int beachPlaceID = await App.BeachPlaceDatabaseInstance.SaveItemAsync(newBeachToAdd);

                List<Entry> entries = new List<Entry> { BeachEntryName, BeachEntryLat, BeachEntryLng };
                Clear_Entry_Fields(entries);
                map.Pins.Clear();
                await DisplayAlert("Success", "Beach saved", "OK");
            }
        }

        /// <summary>
        ///     Once the user successfully adds in a new beach, the Entries they typed in need to be cleared.  This function clears all Text fields in the entry.
        /// </summary>
        /// <param name="entries"> A List of Entry Views whose Text property values need to be set to null ("")</param>
        private void Clear_Entry_Fields(List<Entry> entries)
        {
            foreach(Entry entry in entries)
            {
                entry.Text = "";
            }
        }

        /// <summary>
        ///     To check user's Name input for the beach they wish to save.  The Name of the beach must be unique in the Obs collection and later on,
        ///     the Database table where the BeachPlace objects are stored.
        /// </summary>
        /// <param name="nameToCheck"> The name of the beach to be checked, must be checked for uniqueness and other appropriateness</param>
        /// <returns>returns true if valid, false if not</returns>
        private bool CheckProperBeachName(string nameToCheck)
        {
            //Check to see if the name of the beach, nameToCheck, is in the observable collection (which is basically the Picker view's content)
            foreach (BeachPlace beach in beachCollection)
            {
                if (beach.Name == nameToCheck)
                {
                    DisplayAlert("Error","The beach name \"" + nameToCheck + "\" is already in your list","OK");
                    return false;
                }
            }

            //might want to do a double check and query the DB table of BeachPlaces to see if the name exists in there too 
            //(but if that is case our obs collection should have it)


            return true;
        }

        /// <summary>
        ///     To check the user's input for the beach they wish to save.  The Latitude they entered must be a valid latitude value.
        /// </summary>
        /// <param name="latitudeToCheck">the latitude string value that probably derived the the Latitude Entry view</param>
        /// <returns>returns true if valid, false if not</returns>
        private bool CheckProperLatitudeValue(string latitudeToCheck)
        {
            //should really do TryParse and check for bad input
            if (!double.TryParse(latitudeToCheck, out double numericResult)) 
            {
                DisplayAlert("Error","Latitude value \"" + latitudeToCheck + "\" is not a number","OK");
                return false;
            }
            if (numericResult > 90.000000d || numericResult < -90.000000d)
            {
                DisplayAlert("Error", "Latitude value must be between -90 and 90 degrees", "OK");
                return false;
            }
            return true;
        }

        /// <summary>
        ///     To check the user's input for the beach they wish to save.  The Longitude they entered must be a valid Longitude value.
        /// </summary>
        /// <param name="longitudeToCheck">the Longitude string value that probably derived the the Longitude Entry view<</param>
        /// <returns>returns true if valid, else it returns false</returns>
        private bool CheckProperLongitudeValue(string longitudeToCheck)
        {
            if (!double.TryParse(longitudeToCheck, out double numericResult))
            {
                DisplayAlert("Error", "Latitude value \"" + longitudeToCheck + "\" is not a number", "OK");
                return false;
            }
            if (numericResult > 180.000000d || numericResult < -180.000000d)
            {
                DisplayAlert("Error", "Longitude value must be between -180 and 180 degrees", "OK");
                return false;
            }
            return true;
        }

    }


    //This code is pulled from Documentation:
    //https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/triggers#multi
    /// <summary>
    ///     This helps us make a "require all" multi-trigger.  "The converter code below transforms the Text.Length
    ///     binding into a bool that indicates whether a field is empty or not." "The multi trigger conditions use 
    ///     the converter to turn the Text.Length value into a boolean." this allows us to perform the proper logic
    ///     required to implement a "require all" multi-trigger for length checking of the Entry views.
    /// </summary>
    public class MultiTriggerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if ((int)value > 0) // length > 0 ?
                return true;            // some data has been entered
            else
                return false;            // input is empty
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
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

namespace beach_day
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddBeachPage : ContentPage
	{
        ObservableCollection<BeachPlace> beachCollection; //the user will be able to add to this beach within the add beach function

        public AddBeachPage ()
		{
			InitializeComponent ();
		}

        public AddBeachPage(ObservableCollection<BeachPlace> beachPlaces)
        {
            beachCollection = beachPlaces;

            InitializeComponent();
        }


        private void Add_Beach_Button_Clicked(object sender, EventArgs e)
        {
            //add beach to observable collection and to the DB

            //get the values of the 3 labels, then create a new BeachPlace object and add it  to the observable collection
            //check to see if the 3 labels have appropriate values before accepting, else toss an alert
            //remember, the Name of the beach should be unique and the lat/lng need(?) to be percise to 6 digits lat: -90.000000 to 90.000000 and lng: -180.000000 to 180.000000
            if (CheckProperLatitudeValue(BeachEntryLat.Text) && CheckProperLongitudeValue(BeachEntryLng.Text) && CheckProperBeachName(BeachEntryName.Text))
            {
                string name = BeachEntryName.Text;
                double lat = double.Parse(BeachEntryLat.Text);
                double lng = double.Parse(BeachEntryLng.Text);
                BeachPlace newBeachToAdd = new BeachPlace { Name = name, Latitude = lat, Longitude = lng };

                beachCollection.Add(newBeachToAdd);
                //add the newBeachToAdd to the DB

                List<Entry> entries = new List<Entry> { BeachEntryName, BeachEntryLat, BeachEntryLng };
                Clear_Entry_Fields(entries);
            }
        }
        private void Clear_Entry_Fields(List<Entry> entries)
        {
            foreach(Entry entry in entries)
            {
                entry.Text = "";
            }
        }
        private bool CheckProperBeachName(string nameToCheck)
        {
            //check if its name not in DB (and it shouldn't also already be in the obs collection)
            foreach (BeachPlace beach in beachCollection)
            {
                if (beach.Name == nameToCheck)
                {
                    DisplayAlert("Error","The beach name \"" + nameToCheck + "\" is already used","OK");
                    return false;
                }
            }
            return true;
        }
        private bool CheckProperLatitudeValue(string latitudeToCheck)
        {
            //should really do TryParse and check for bad input
            if (!float.TryParse(latitudeToCheck, out float numericResult)) //************If user input is "90.000001" then numericResult is 90, this percision sucks, need to use double 
            {
                DisplayAlert("Error","Latitude value \"" + latitudeToCheck + "\" is not a number","OK");
                return false;
            }
            if (numericResult > 90.000000f || numericResult < -90.000000f)
            {
                DisplayAlert("Error", "Latitude value must be between -90 and 90 degrees", "OK");
                return false;
            }
            return true;
        }
        private bool CheckProperLongitudeValue(string longitudeToCheck)
        {
            //should really do TryParse and check for bad input

            if (!float.TryParse(longitudeToCheck, out float numericResult))
            {
                DisplayAlert("Error", "Latitude value \"" + longitudeToCheck + "\" is not a number", "OK");
                return false;
            }
            if (numericResult > 180.000000f || numericResult < -180.000000f)
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
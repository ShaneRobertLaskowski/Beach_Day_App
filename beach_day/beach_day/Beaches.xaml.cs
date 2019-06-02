using beach_day.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace beach_day
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Beaches : ContentPage
	{
        ObservableCollection<BeachPlace> beachCollection;
        Map map;

        //overload this constructor, made the binding context set to the passed in observable collection
		public Beaches ()
		{
			InitializeComponent ();
		}

        //because this page is pushAsynced, we want to pass the observable collection so that when we navigate back, 
        //changes will reflect in the viewlist too.  Remember, objects are passed by reference (even without ref keyword)
        //*note, if we used ref keyword, then our beachPlaces parameter will also be a reference to the argument's variable, for now its a new reference to our object
        public Beaches(ObservableCollection<BeachPlace> beachPlaces)
        {
            beachCollection = beachPlaces;

            InitializeComponent();
            AddGoogleMap();

            //BeachPlace beach8 = new BeachPlace { Name = "Test Beach", Latitude = 32.750000, Longitude = -117.252000 }; // TEST
            //beachPlaces.Add(beach8);

            BeachPicker.ItemsSource = beachCollection; //binds the observable collection to the Picker view

            //BindingContext = beachPlaces; //???
        }

        private void AddGoogleMap()
        {
            map = new Map(MapSpan.FromCenterAndRadius(new Position(32.7157, -117.1611), Distance.FromMiles(75.0))) //var map
            {
                IsShowingUser = true,
                MapType = MapType.Hybrid,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            PageGridLayout.Children.Add(map, 0, 0);
            Grid.SetColumnSpan(map, 3);
        }


        /// <summary>
        ///     This function is ran every time the picker's selected index changes.  Generally, the this function is ran when
        ///     user selects a beach from the Picker view.  It is also called whenever an item is deleted from the Picker too.  
        ///     Whenever a user selects a beach, a map marker is placed on the beaches location.  in the event of the user deleting
        ///     a beach, the function must NOT try to place a marker.  (Picker view doesn't come with a simple 'selected item event').
        /// </summary>
        /// <param name="sender">The Picker view that the user is selecting from</param>
        /// <param name="e"></param>
        private void Beach_Picked(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;
            BeachPlace beachSelected = (BeachPlace)picker.SelectedItem;
            Position position;
            string NameOfBeachSelected;
            bool pickerIsFocused = picker.IsFocused; //to help differentiate whether this event/function is being called for either the Picker's
                                                     //delete or select of functionality...
                                                     //if the Picker view is focused, then we know the user is selecting a beach, if the user is deleting
                                                     //if the Picker view is not focused, then we know the user is clicking the delete button. **this is ugly

            if (beachSelected != null && pickerIsFocused)
            {
                NameOfBeachSelected = beachSelected.Name;
                position = new Position(beachSelected.Latitude, beachSelected.Longitude); // Latitude, Longitude
                map.Pins.Clear();
                AddPinToMap(NameOfBeachSelected, position, map);
                map.MoveToRegion(new MapSpan(position, 0.0125, 0.0125)); //the last 2 arguments, are lat/lng values, used to define a zoom level (circle radius?) documentation unclear
                map.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMiles(0.5)));
            }
        }

        private void AddPinToMap(string NameOfLocation, Position position, Map targetedMap)
        {
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = position,
                Label = NameOfLocation,
                Address = "insert address or lat/lng here"
            };
            targetedMap.Pins.Add(pin);
            
        }

        private async void Toolbar_AddBeach_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddBeachPage(beachCollection)); //beachCollection -> use this a argument
        }

        //delete the beach from the observable collection/Picker and from the DB table of BeachPlaces
        private async void Button_DeleteBeach_Clicked(object sender, EventArgs e)
        {
            //delete the object first from the DB then from the Picker/observable collection
            if(await DisplayAlert("Warning","Are you sure you want to delete this beach?", "Yes", "No"))
            {
                BeachPlace beachToRemove = (BeachPlace)BeachPicker.SelectedItem;
                beachCollection.Remove(beachToRemove); //removes the selected item (from both picker and obs collection)
                BeachPicker.SelectedItem = null; //"deselects" any item selected by the Picker view.

                //deletes the beach from the DB table of BeachPlaces
                int beachPlaceID = await App.BeachPlaceDatabaseInstance.DeleteItemAsync(beachToRemove);

                map.Pins.Clear(); //the map pin is placed on the map => delete it, ***note, this deletes all pins, only 1 pin should be on the map 
                                  //in the future, if multiple pins are on the map that want to be kept on, we have a problem, we going to have to
                                  //delete a specific pin, going to have to figure out how to pass the currently selected beaches' pin at just remove that
                                  //perhaps iterate through the pins and find the one with the same lat/lng as the Selected Beach's.

            }
        }
    }
}
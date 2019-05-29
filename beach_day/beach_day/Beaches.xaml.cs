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

            BeachPlace beach8 = new BeachPlace { Name = "Test Beach", Latitude = 32.750000, Longitude = -117.252000 }; // TEST
            beachPlaces.Add(beach8);

            BeachPicker.ItemsSource = beachCollection; //binds the observable collection to the Picker view

            //BindingContext = beachPlaces; //???
        }

        private void AddGoogleMap()
        {
            var map = new Map(MapSpan.FromCenterAndRadius(new Position(32.7157, -117.1611), Distance.FromMiles(75.0)))
            {
                IsShowingUser = true,
                MapType = MapType.Hybrid,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            PageGridLayout.Children.Add(map, 0, 0);
            Grid.SetColumnSpan(map, 3);
        }


        //picker function that occurs when Beach is selected in the picker
        private void Beach_Picked(object sender, EventArgs e)
        {

        }

        private async void Toolbar_AddBeach_Clicked(object sender, EventArgs e)
        {
            
            await Navigation.PushAsync(new AddBeachPage(beachCollection)); //beachCollection -> use this a argument
        }

        //delete the beach from the observable collection/Picker, eventually this will also delete from a DB table
        private async void Button_DeletBeach_Clicked(object sender, EventArgs e)
        {
            //delete the object first from the DB then from the Picker/observable collection
            if(await DisplayAlert("Warning","Are you sure you want to delete this beach?", "Yes", "No"))
            {
                beachCollection.Remove((BeachPlace)BeachPicker.SelectedItem); //removes the selected item (from both picker and obs collection)
                BeachPicker.SelectedItem = null; //"deselects" any item selected by the Picker view.
            }
        }
    }
}
using beach_day.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace beach_day
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Beaches : ContentPage
	{
        //overload this constructor, made the binding context set to the passed in observable collection
		public Beaches ()
		{
			InitializeComponent ();
		}

        //because this page is pushAsynced, we want to pass the observable collection so that when we navigate back, 
        //changes will reflect in the viewlist too.
        public Beaches(List<BeachPlace> beachPlaces)
        {
            //make the list into observable collection
            ObservableCollection<BeachPlace> beachesObs = new ObservableCollection<BeachPlace>(beachPlaces);
            InitializeComponent();
            BindingContext = beachesObs;
        }
        

        //picker function that occurs when Beach is selected in the picker
        private void Beach_Picked(object sender, EventArgs e)
        {

        }

        private async void Toolbar_AddBeach_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddBeachPage());
        }

        //delete the beach from the observable collection, eventually this will also delete from a DB table
        private void Button_DeletBeach_Clicked(object sender, EventArgs e)
        {

        }
    }
}
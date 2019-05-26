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
    }
}
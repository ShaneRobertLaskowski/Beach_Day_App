using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace beach_day
{
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();
            Detail = new NavigationPage(new BeachSelectPage()); //change this the first page you want the user to see

        }

        private void BeachSelect_Creation_Button(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new BeachSelectPage());
            IsPresented = false; //hides the MainPage (master detail page)
        }
        private void BeachItemChecklist_Creation_Button(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new ItemChecklist());
            IsPresented = false;
        }
        private void BeachFacts_Creation_Button(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new BeachFacts());
            IsPresented = false;
        }
        private void About_Creation_Button(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new About());
            IsPresented = false;
        }
    }
}

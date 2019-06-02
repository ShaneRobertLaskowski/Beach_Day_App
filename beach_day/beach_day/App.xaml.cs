using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace beach_day
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        //this code block creates our cross-platform SQLite database instance if its not already created
        //because its static the app will only have to open and close 1 connection for our app's entire use of it.
        //Therefore, no need to open/close connection to the database with every query operation
        static ItemListDatabase itemDatabase;
        public static ItemListDatabase ItemDatabaseInstance
        {
            get
            {
                if(itemDatabase == null)
                {
                    itemDatabase = new ItemListDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ItemSQLite.db3"));
                }
                return itemDatabase;
            }

        }

        //second database for BeachPlace objects
        static BeachPlaceDatabase beachPlaceDatabase;
        public static BeachPlaceDatabase BeachPlaceDatabaseInstance
        {
            get
            {
                if (beachPlaceDatabase == null)
                {
                    beachPlaceDatabase = new BeachPlaceDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BeachPlaceSQLite.db3"));
                }
                return beachPlaceDatabase;
            }

        }


        protected override void OnStart()
        {
            // Handle when your app starts

            //place other "app secrets" here 
            AppCenter.Start("android=1d382a07-2b8f-4eb0-aac6-cb70a4d5b80e;", typeof(Analytics), typeof(Crashes));

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

        protected override void OnStart()
        {
            // Handle when your app starts

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

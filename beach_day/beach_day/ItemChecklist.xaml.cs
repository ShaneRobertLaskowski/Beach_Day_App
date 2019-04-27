using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace beach_day
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemChecklist : ContentPage
	{
        private static ObservableCollection<ChecklistItemModel> items = new ObservableCollection<ChecklistItemModel>();


        public ItemChecklist ()
		{
			InitializeComponent ();

            try
            {
                Populate_CheckList();
            }
            catch (Exception ex)
            { 
                var TheException = ex;
            }

        }

        private async void Populate_CheckList() //make this async if adding in database functionality (it has tasks to be awaited)
        {
            /*
            var item1 = new ChecklistItemModel { Name = "SunScreen", ID = 1 };
            var item2 = new ChecklistItemModel { Name = "Towel", ID = 2 };
            var item3 = new ChecklistItemModel { Name = "Umbrella", ID = 3 };
            var item4 = new ChecklistItemModel { Name = "Volley Ball", ID = 4 };
            var item5 = new ChecklistItemModel { Name = "Flip Flops", ID = 5 };
            var item6 = new ChecklistItemModel { Name = "Water", ID = 6 };
            var item7 = new ChecklistItemModel { Name = "LunchBox", ID = 7 };
            */

            //run query to get checklist List, pass it to ObservableCollection, assign that the ViewList
            //***********TEST*************            
            var allItems = await App.ItemDatabaseInstance.GetAllChecklistItems();

            /*var itemResult = await App.ItemDatabaseInstance.GetItemWithName("cat"); //*****when an item with same name as this isn't in the DB, the app tosses "Constraint" exception
            if (itemResult.Count != 0)
            {
                //item found --> can't add in item

                var testInt = 2 + 2;
                //this alert is never displayed
                DisplayAlert("Error","Item Already Added to List","OK");  //if this is awaited then viewlist is never initialized
            }
            else
            {
                //There is no item that has this name --> create a CheckListItemModel and insert it into the DB
                var userNewItem = new ChecklistItemModel {Name = "cat" }; //*************************** there is already a "dog" record in DB, but the Name column is UNIQUE, ==> "constraint" exception thrown

                int ItemId = await App.ItemDatabaseInstance.SaveItemAsync(userNewItem);
            }
            */
           
            /*
            List<ChecklistItemModel> testItemList = new List<ChecklistItemModel>
            {
                item1, item2, item3, item4, item5, item6, item7
            };
            // ObservableCollection<ChecklistItemModel> items = new ObservableCollection<ChecklistItemModel>(testItemList);
            */
            items = new ObservableCollection<ChecklistItemModel>(allItems); //testItemList
            ItemViewList.ItemsSource = items;
       
        }

        //adds item to the displaying viewlist by adding data to the observable collection "items"
        private async void Item_Added_Clicked(object sender, EventArgs e)
        {

            //grab the Text content of the Entry
            var userItem = UserItemInput.Text;
            var itemResult = await App.ItemDatabaseInstance.GetItemWithName(userItem); //*****when an item with same name as this isn't in the DB, the app tosses "Constraint" exception

            if (itemResult.Count != 0)
            {
                //item found --> can't add in item
                var testInt = 2 + 2;
                await DisplayAlert("Error", "Item Already Added to List", "OK");  //if this is awaited then viewlist is never initialized
            }
            else
            {
                //There is no item that has this name --> create a CheckListItemModel and insert it into the DB
                var userNewItem = new ChecklistItemModel { Name = userItem, ID = 0 };
                int ItemId = await App.ItemDatabaseInstance.SaveItemAsync(userNewItem);
                items.Insert(0, userNewItem);
                Clear_Entry_Field();

            }


            //update the observable collection by grabbing the listView's itemsource and appending a new CheckListItem with Name property equal to the Text value passed
            //var newItem = new ChecklistItemModel { Name = userItem, ID = 0 };
            //items.Insert(0, newItem);
            //Clear_Entry_Field();
        }

        //deletes the item passed via command parameter from the observable collection, which reflects in the displayed viewlist, and deletes from the DB too
        private async void MenuItem_Delete_Clicked(object sender, EventArgs e)
        {
            var menuItem = (MenuItem)sender;
            var item = (ChecklistItemModel)menuItem.CommandParameter;
            items.Remove(item);
            var ItemId = await App.ItemDatabaseInstance.DeleteItemAsync(item);
        }

        private void Clear_Entry_Field()
        {
            UserItemInput.Text = "";
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

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
           
            var allItems = await App.ItemDatabaseInstance.GetAllChecklistItems();
            items = new ObservableCollection<ChecklistItemModel>(allItems); //testItemList
            ItemViewList.ItemsSource = items;
       
        }

        //adds item to the displaying viewlist by adding data to the observable collection "items"
        private async void Item_Added_Clicked(object sender, EventArgs e)
        {

            //grab the Text content of the Entry
            var userItem = UserItemInput.Text;

            if(string.IsNullOrWhiteSpace(userItem))
            {
                await DisplayAlert("Error", "Input cannot be empty", "OK");
                Clear_Entry_Field(UserItemInput);
                return;
            }

            //check DB for any instances of items with same name as userItem's value
            var itemResult = await App.ItemDatabaseInstance.GetItemWithName(userItem); 

            if (itemResult.Count != 0)
            {
                //item found --> can't add item to DB
                await DisplayAlert("Error", "Item Already Added to List", "OK");
            }
            else
            {
                //There is no item that has this name --> create a CheckListItemModel and insert it into the DB
                //ID is set to 0, this lets SQLite's autoincrement contraint "know" that it needs to be modified to proper value
                var userNewItem = new ChecklistItemModel { Name = userItem, ID = 0 }; 
                int ItemId = await App.ItemDatabaseInstance.SaveItemAsync(userNewItem);
                items.Insert(0, userNewItem);
                Clear_Entry_Field(UserItemInput);

            }
        }

        //deletes the item passed via command parameter from the observable collection, which reflects in the displayed viewlist, and deletes from the DB too
        private async void MenuItem_Delete_Clicked(object sender, EventArgs e)
        {
            var menuItem = (MenuItem)sender;
            var item = (ChecklistItemModel)menuItem.CommandParameter;
            items.Remove(item);
            var ItemId = await App.ItemDatabaseInstance.DeleteItemAsync(item);
        }


        private void Clear_Entry_Field(Entry e)
        {
            e.Text = "";
        }

        /// <summary>
        ///     passes the stack layout and disables/enables the strikethrough of the Label view that must
        ///     be the first child view of this layout.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StrikeThroughAddOrRemove(object sender, EventArgs e)
        {
            var layout = (StackLayout) sender;

            var StackLayoutContents = layout.Children;
            Label firstLabel; //= (Label) StackLayoutContents[0]; //***HardCode -> the first item of the layout must be the Label view.
            int labelIndex = FindFirstLabelView(StackLayoutContents);

            if (labelIndex != -1)
            {
                firstLabel = (Label)StackLayoutContents[labelIndex];
                if (firstLabel.TextDecorations == Xamarin.Forms.TextDecorations.Strikethrough)
                    firstLabel.TextDecorations = Xamarin.Forms.TextDecorations.None;
                else firstLabel.TextDecorations = Xamarin.Forms.TextDecorations.Strikethrough;

                VibratePhone();
            }
            //else throw exception (could not find a Label view within the stacklayout)
        }

        /// <summary>
        ///     Finds the first Label view within IList
        /// </summary>
        /// <param name="layout"> an IList container of Views</param>
        /// <returns> The index of the first Label view, or -1 if none are found</returns>
        private int FindFirstLabelView(IList<View> layout)
        {

            for(int i = 0; i < layout.Count; i++)
            {
                
                if(layout[i].ToString() == typeof(Label).ToString())
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        ///     Vibrates the phone
        /// </summary>
        private void VibratePhone()
        {
            try
            {
                // Use default vibration length
                //Vibration.Vibrate();

                // Or use specified time
                var duration = TimeSpan.FromMilliseconds(50);
                Vibration.Vibrate(duration);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }

    }
}
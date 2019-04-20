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
            Populate_CheckList();

        }

        private void Populate_CheckList()
        {

            var item1 = new ChecklistItemModel { Name = "SunScreen", ID = 1 };
            var item2 = new ChecklistItemModel { Name = "Towel", ID = 2 };
            var item3 = new ChecklistItemModel { Name = "Umbrella", ID = 3 };
            var item4 = new ChecklistItemModel { Name = "Volley Ball", ID = 4 };
            var item5 = new ChecklistItemModel { Name = "Flip Flops", ID = 5 };
            var item6 = new ChecklistItemModel { Name = "Water", ID = 6 };
            var item7 = new ChecklistItemModel { Name = "LunchBox", ID = 7 };
            //run query to get checklist List, pass it to ObservableCollection, assign that the ViewList
            List<ChecklistItemModel> testItemList = new List<ChecklistItemModel>
            {
                item1, item2, item3, item4, item5, item6, item7
            };
            // ObservableCollection<ChecklistItemModel> items = new ObservableCollection<ChecklistItemModel>(testItemList);

            items = new ObservableCollection<ChecklistItemModel>(testItemList);
            ItemViewList.ItemsSource = items;
       
        }

        //adds item to the displaying viewlist by adding data to the observable collection "items"
        private void Item_Added_Clicked(object sender, EventArgs e)
        {

            //grab the Text content of the Entry
            var userItem = UserItemInput.Text;
            
            /*
            add this code after next phase of development
            try to add to the Database
            if failure, display alert (such as it already in the list)
            */

            //update the observable collection by grabbing the listView's itemsource and appending a new CheckListItem with Name property equal to the Text value passed
            var newItem = new ChecklistItemModel { Name = userItem, ID = 17 };
            items.Insert(0, newItem);
            Clear_Entry_Field();
        }

        //deletes the item passed via command parameter from the observable collection, which reflects in the displayed viewlist
        private void MenuItem_Delete_Clicked(object sender, EventArgs e)
        {
            var menuItem = (MenuItem)sender;
            items.Remove((ChecklistItemModel)menuItem.CommandParameter);

        }

        private void Clear_Entry_Field()
        {
            UserItemInput.Text = "";
        }
    }
}
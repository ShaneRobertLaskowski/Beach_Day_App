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
		public ItemChecklist ()
		{
			InitializeComponent ();
		}

        private void CheckListPage_Appearing(object sender, EventArgs e)
        {

            var item1 = new ChecklistItemModel { Name = "SunScreen", ID = 1 };
            var item2 = new ChecklistItemModel { Name = "Towel", ID = 2 };
            var item3 = new ChecklistItemModel { Name = "Umbrella", ID = 3 };
            //run query to get checklist List, pass it to ObservableCollection, assign that the ViewList
            List<ChecklistItemModel> testItemList = new List<ChecklistItemModel>
            {
                item1, item2, item3
            };
            ObservableCollection<ChecklistItemModel> items = new ObservableCollection<ChecklistItemModel>(testItemList);
            ItemViewList.ItemsSource = items;




        }
    }
}
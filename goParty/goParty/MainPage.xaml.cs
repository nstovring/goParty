using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using goParty.Models;
namespace goParty.Pages
{
	public partial class MainPage : ContentPage
	{

        public ListView ListView { get { return listView; } }


        public MainPage()
		{
			InitializeComponent();

            var masterPageItems = new List<MasterPageItem>();

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Task List",
                IconSource = "todo.png",
                TargetType = typeof(TaskList)
            });

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Find Parties",
                IconSource = "contacts.png",
                TargetType = typeof(MapPage)
            });

            masterPageItems.Add(new MasterPageItem
            {
                Title = "TestPage",
                IconSource = "hamburger.png",
                TargetType = typeof(TestPage)
            });

            //masterPageItems.Add(new MasterPageItem
            //{
            //    Title = "Contacts",
            //    IconSource = "contacts.png",
            //    TargetType = typeof(ContactsPage)
            //});
            //masterPageItems.Add(new MasterPageItem
            //{
            //    Title = "TodoList",
            //    IconSource = "todo.png",
            //    TargetType = typeof(TodoListPage)
            //});
            //masterPageItems.Add(new MasterPageItem
            //{
            //    Title = "Reminders",
            //    IconSource = "reminders.png",
            //    TargetType = typeof(ReminderPage)
            //});

            listView.ItemsSource = masterPageItems;

        }
	}
}

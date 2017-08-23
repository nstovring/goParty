using goParty.Models;
using goParty.Pages;
using System;
using System.Collections.Generic;
using System.Text;

namespace goParty.Helpers
{
    static class MasterDetailPageManager
    {

        public static MasterPageItem ProfilePage = new MasterPageItem
        {
            Title = "Profile Page",
            IconSource = "hamburger.png",
            TargetType = typeof(ProfilePage)
        };

        public static MasterPageItem TaskListPage = new MasterPageItem
        {
            Title = "Profile Page",
            IconSource = "hamburger.png",
            TargetType = typeof(TaskList)
        };

        public static MasterPageItem MapPage = new MasterPageItem
        {
            Title = "Profile Page",
            IconSource = "hamburger.png",
            TargetType = typeof(MapPage)
        };

        public static MasterPageItem CreatePartyPage = new MasterPageItem
        {
            Title = "Profile Page",
            IconSource = "hamburger.png",
            TargetType = typeof(CreatePartyPage)
        };

        public static MasterPageItem TestPage = new MasterPageItem
        {
            Title = "Profile Page",
            IconSource = "hamburger.png",
            TargetType = typeof(TestPage)
        };


        public static List<MasterPageItem> PageItems()
        {
            var masterPageItems = new List<MasterPageItem>();

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Profile Page",
                IconSource = "hamburger.png",
                TargetType = typeof(ProfilePage)
            });

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
                Title = "Create Party",
                IconSource = "hamburger.png",
                TargetType = typeof(CreatePartyPage)
            });

            masterPageItems.Add(new MasterPageItem
            {
                Title = "TestPage",
                IconSource = "hamburger.png",
                TargetType = typeof(TestPage)
            });

            return masterPageItems;
        }
    }
}

using goParty.Models;
using goParty.Pages;
using goParty.Pages.ManagementPages.AttendeePages;
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


        public static ObservableRangeCollection<MasterPageItem> PageItems()
        {
            var masterPageItems = new ObservableRangeCollection<MasterPageItem>();

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Party List",
                IconSource = "partylist.png",
                TargetType = typeof(TabbedContentPage)
            });


            masterPageItems.Add(new MasterPageItem
            {
                Title = "Party Map",
                IconSource = "partymap.png",
                TargetType = typeof(MapPage)
            });

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Create Party",
                IconSource = "createparty.png",
                TargetType = typeof(CreatePartyPage)
            });

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Events",
                IconSource = "events.png",
                TargetType = typeof(AttendeePage)
            });

            masterPageItems.Add(new MasterPageItem
            {
                Title = "My Page",
                IconSource = "mypage.png",
                TargetType = typeof(ProfilePage)
            });


            masterPageItems.Add(new MasterPageItem
            {
                Title = "Settings",
                IconSource = "settings.png",
                TargetType = typeof(ProfilePage)
            });

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Log Out",
                IconSource = "signout.png",
                TargetType = typeof(EntryPage)
            });


            return masterPageItems;
        }
    }
}

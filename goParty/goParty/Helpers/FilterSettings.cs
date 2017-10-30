using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace goParty.Helpers
{
    static class FilterSettings
    {
        public static Position SearchPosition;
        public static int MinAge;
        public static int MaxAge;
        public static int Price;

        public delegate void OnFilterChanged(object sender);
        public static OnFilterChanged FilterChanged;
    }
}

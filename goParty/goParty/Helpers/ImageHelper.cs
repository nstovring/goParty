using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace goParty.Helpers
{
    static class ImageHelper
    {
        public class ImageHelperItem{
            public ImageSource image;
            public string imageId;
        }
        public static List<ImageHelperItem> LoadedImages = new List<ImageHelperItem>();
    }
}

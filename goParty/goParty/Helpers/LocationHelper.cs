using Plugin.Geolocator;
using System;
using Xamarin.Forms.Maps;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace goParty.Helpers
{
    public static class LocationHelper
    {

        public static async Task<Position> GetLocation()
        {
            var position = new Position(55.669989, 12.572854);
            try
            {
                var locator = CrossGeolocator.Current;
                var pos = await locator.GetPositionAsync(TimeSpan.FromMilliseconds(50));
                position = new Position(pos.Latitude, pos.Longitude);
                //Overriding to Copenhagen
                position = new Position(55.669989, 12.572854);
            }
            catch (Exception error)
            {
                Debug.WriteLine("[$] Location Issue" + error.Message);
            }
            return position;
        }


        public static string convertDistanceToClosestMeasuringUnit(double distance)
        {
            int decmialOffset = distance > 1 ? 0 : 2;
            string unit = distance > 1 ? "Km" : "M";
            string tempString = distance.ToString();
            string newString = new string(tempString.ToCharArray(), decmialOffset, 4);
            return newString +unit;
        }

       

        public static string distance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            if (unit == 'K')
            {
                dist = dist * 1.609344;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }
            return convertDistanceToClosestMeasuringUnit(dist);
        }

        private static double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        private static double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
    }
}

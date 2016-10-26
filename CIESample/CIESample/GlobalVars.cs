using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CIESample
{
    class GlobalVars
    {

        public class Variables
        {
            public static string currentID { get; set; }
            public static string currentPosterPath { get; set; }
            public static string currentOriginalTitle { get; set; }
            public static string currentOverview { get; set; }
            public static string currentReleaseDate { get; set; }
        }

    }
}
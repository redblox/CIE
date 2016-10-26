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
using System.Threading.Tasks;
using CIESample.SAL;

namespace CIESample.BL
{
    class DataManager
    {

        public static async Task<string> GetNowPlayingJSON()
        {
            return await Services.GetNowPlayingJSON();
        }

        public static async Task<string> GetTopRatedJSON()
        {
            return await Services.GetTopRatedJSON();
        }

        public static async Task<string> GetPopularJSON()
        {
            return await Services.GetPopularJSON();
        }
        public static async Task<string> GetSimilarJSON(string idPassed)
        {
            return await Services.GetSimilarJSON(idPassed);
        }
        
    }
}
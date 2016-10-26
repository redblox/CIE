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
using System.Threading;
using Android.Util;
using Android.Content.PM;


namespace CIESample
{
    [Activity(MainLauncher = true, NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/Theme.Splash")]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            this.Window.AddFlags(WindowManagerFlags.Fullscreen);
            base.OnCreate(bundle);

            Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);

            StartActivity(typeof(MainActivity));
        }
    }
}








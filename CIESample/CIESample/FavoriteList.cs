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
using Org.Json;
using Newtonsoft.Json;
using Android.Content.PM;
using System.Threading;

namespace CIESample
{
    [Activity(Label = "FavoriteList", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@android:style/Theme.NoTitleBar")]
    public class FavoriteList : Activity
    {

        public List<string> ls = new List<string>();

        GridView gridview;

        public static Activity fa;

        public static Boolean currentlyRunning = true;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.FavoritesList);

            fa = this;

            GetFilms();
        }

        async Task<List<Models.Favorites>> GetFilms()
        {
            var FavFilms = await DAL.Database.GetFavFilms();

            for (var i = 0; i < FavFilms.Count; i++)
            {
                Console.WriteLine((FavFilms[i].Id)+ " (FavFilms[i].Id)");
                ls.Add(FavFilms[i].poster_path);
            };

            gridview = FindViewById<GridView>(Resource.Id.gridview);
            gridview.Adapter = new ImageAdapter(this, ls);

            gridview.ItemClick += (sender, args) =>
            {
                GlobalVars.Variables.currentID = FavFilms[args.Position].Id.ToString();
                GlobalVars.Variables.currentPosterPath = FavFilms[args.Position].poster_path.ToString();
                GlobalVars.Variables.currentOriginalTitle = FavFilms[args.Position].original_title.ToString();
                GlobalVars.Variables.currentOverview = FavFilms[args.Position].overview.ToString();
                GlobalVars.Variables.currentReleaseDate = FavFilms[args.Position].release_date.ToString();

                ProgressDialog progressDialog = ProgressDialog.Show(this, "", "Loading...", true);
                progressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
                new Thread(new ThreadStart(delegate
                {
                    var selectIntent = new Intent(this, typeof(FavoriteMovie));
                    StartActivity(selectIntent);
                    RunOnUiThread(() => progressDialog.Dismiss());
                })).Start();

              
             };

            return FavFilms;
        }
    }
}
using Android.App;
using Android.OS;
using Android.Widget;
using Org.Json;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading;
using Android.Content;
using Android.Content.PM;

namespace CIESample
{
    [Activity(Label = "CIESample", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@android:style/Theme.NoTitleBar")]

    public class MainActivity : Activity
    {
        public List<string> ls = new List<string>();
        public List<string> ls2 = new List<string>();
        public List<string> ls3 = new List<string>();

        public ProgressDialog lProgressDlg { get; set; }

        public static Activity fa;

        protected override void OnCreate(Bundle bundle)
        {

            fa = this;

            base.OnCreate(bundle);

			SetContentView(Resource.Layout.Main);

            Button loadFavorites = FindViewById<Button>(Resource.Id.buttonLoad);

            loadFavorites.Click += LoadFavorites_Click;
            
            var LoadTopRated_ = LoadTopRated();
            var LoadPopularPlaying_ = LoadPopularPlaying();
            var LoadNowPlaying_ = LoadNowPlaying();
        }

        private void LoadFavorites_Click(object sender, EventArgs e)
        {
            ProgressDialog progressDialog = ProgressDialog.Show(this, "", "Loading...", true);
            progressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
            new Thread(new ThreadStart(delegate
            {
                var selectIntent = new Intent(this, typeof(FavoriteList));
                StartActivity(selectIntent);
                RunOnUiThread(() => progressDialog.Dismiss());
            })).Start();
        }

        async Task<string> LoadTopRated()
        { 
            var response = await BL.DataManager.GetTopRatedJSON();

            JSONObject lUserObj = new JSONObject(response);

            Models.RootObject subjects = JsonConvert.DeserializeObject<Models.RootObject>(response);

            for (var i = 0; i < subjects.results.Count - 2; i++)
            {
                ls.Add(subjects.results[i].poster_path);
            };

            var gridview = FindViewById<GridView>(Resource.Id.gridview);
            gridview.Adapter = new ImageAdapter(this, ls);

            gridview.ItemClick += (sender, args) =>
            {
                GlobalVars.Variables.currentID = subjects.results[args.Position].id.ToString();
                GlobalVars.Variables.currentPosterPath = subjects.results[args.Position].poster_path.ToString();
                GlobalVars.Variables.currentOriginalTitle = subjects.results[args.Position].original_title.ToString();
                GlobalVars.Variables.currentOverview = subjects.results[args.Position].overview.ToString();
                GlobalVars.Variables.currentReleaseDate = subjects.results[args.Position].release_date.ToString();

                ProgressDialog progressDialog = ProgressDialog.Show(this, "", "Loading...", true);
                progressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
                new Thread(new ThreadStart(delegate
                {
                    var selectIntent = new Intent(this, typeof(SelectedMovie));
                    StartActivity(selectIntent);
                    RunOnUiThread(() => progressDialog.Dismiss());
                })).Start();

                
            };

            return response;
        }

        async Task<string> LoadPopularPlaying()
        {

            var response = await BL.DataManager.GetPopularJSON();

            JSONObject lUserObj = new JSONObject(response);

            Models.RootObject subjects = JsonConvert.DeserializeObject<Models.RootObject>(response);

            for (var i = 0; i < subjects.results.Count - 2; i++)
            {
                ls2.Add(subjects.results[i].poster_path);
            };

            var gridview2 = FindViewById<GridView>(Resource.Id.gridview2);
            gridview2.Adapter = new ImageAdapter(this, ls2);

            gridview2.ItemClick += (sender, args) =>
            {
                GlobalVars.Variables.currentID = subjects.results[args.Position].id.ToString();
                GlobalVars.Variables.currentPosterPath = subjects.results[args.Position].poster_path.ToString();
                GlobalVars.Variables.currentOriginalTitle = subjects.results[args.Position].original_title.ToString();
                GlobalVars.Variables.currentOverview = subjects.results[args.Position].overview.ToString();
                GlobalVars.Variables.currentReleaseDate = subjects.results[args.Position].release_date.ToString();

                ProgressDialog progressDialog = ProgressDialog.Show(this, "", "Loading...", true);
                progressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
                new Thread(new ThreadStart(delegate
                {
                    var selectIntent = new Intent(this, typeof(SelectedMovie));
                    StartActivity(selectIntent);
                    RunOnUiThread(() => progressDialog.Dismiss());
                })).Start();
            };

            return response;
        }

        async Task<string> LoadNowPlaying()
        {

            var response = await BL.DataManager.GetNowPlayingJSON();

            JSONObject lUserObj = new JSONObject(response);

            Models.RootObject subjects = JsonConvert.DeserializeObject<Models.RootObject>(response);

            for (var i = 0; i < subjects.results.Count - 2; i++)
            {
                ls3.Add(subjects.results[i].poster_path);
            };

            var gridview3 = FindViewById<GridView>(Resource.Id.gridview3);
            gridview3.Adapter = new ImageAdapter(this, ls3);

            gridview3.ItemClick += (sender, args) =>
            {
                GlobalVars.Variables.currentID = subjects.results[args.Position].id.ToString();
                GlobalVars.Variables.currentPosterPath = subjects.results[args.Position].poster_path.ToString();
                GlobalVars.Variables.currentOriginalTitle = subjects.results[args.Position].original_title.ToString();
                GlobalVars.Variables.currentOverview = subjects.results[args.Position].overview.ToString();
                GlobalVars.Variables.currentReleaseDate = subjects.results[args.Position].release_date.ToString();

                ProgressDialog progressDialog = ProgressDialog.Show(this, "", "Loading...", true);
                progressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
                new Thread(new ThreadStart(delegate
                {
                    var selectIntent = new Intent(this, typeof(SelectedMovie));
                    StartActivity(selectIntent);
                    RunOnUiThread(() => progressDialog.Dismiss());
                })).Start();
            };

            return response;
        }
    }
}


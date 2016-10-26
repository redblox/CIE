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
using Android.Graphics;
using System.Net;
using System.Threading.Tasks;
using Org.Json;
using Newtonsoft.Json;
using Android.Content.PM;
using System.Threading;

namespace CIESample
{
    [Activity(Label = "FavoriteMovie", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@android:style/Theme.NoTitleBar")]
    public class FavoriteMovie : Activity
    {
        public List<string> ls = new List<string>();
        public ImageView MoviePoster;
        Button FavBtn;
        public static Activity fa;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.FavoriteMovie);

            TextView title = FindViewById<TextView>(Resource.Id.title_tv);
            title.Text = GlobalVars.Variables.currentOriginalTitle;

            TextView overview = FindViewById<TextView>(Resource.Id.overview_tv);
            overview.Text = GlobalVars.Variables.currentOverview;

            TextView ReleaseDate = FindViewById<TextView>(Resource.Id.release_tv);
            ReleaseDate.Text = "Release Date: " + GlobalVars.Variables.currentReleaseDate;

            FavBtn = FindViewById<Button>(Resource.Id.buttonSave);

            FavBtn.Click += Btn_Click;

            MoviePoster = FindViewById<ImageView>(Resource.Id.Movie_iv);
            var imageBitmap = GetImageBitmapFromUrl("http://image.tmdb.org/t/p/w150" + GlobalVars.Variables.currentPosterPath);
            MoviePoster.SetImageBitmap(imageBitmap);

            var LoadSimilar_ = LoadSimilar();
        }

        private async void Btn_Click(object sender, EventArgs e)
        {
            if (FavBtn.Text == "Delete this favorite")
            {
                Toast.MakeText(this, "You've deleted this favorite.", ToastLength.Long).Show();
                await DAL.Database.deleteFavFilm(Convert.ToInt32(GlobalVars.Variables.currentID));
                FavBtn.Text = "Save to favorites";
                FavoriteList.fa.Recreate();
                return;
            }
            else if(FavBtn.Text == "Save to favorites")
            {
                Toast.MakeText(this, "Saved to your favorites.", ToastLength.Long).Show();
                await DAL.Database.StroreFav(GlobalVars.Variables.currentOriginalTitle, GlobalVars.Variables.currentOverview, GlobalVars.Variables.currentPosterPath, GlobalVars.Variables.currentReleaseDate, GlobalVars.Variables.currentID);
                FavBtn.Text = "Delete this favorite";
                FavoriteList.fa.Recreate();
                return;
            }   
        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }

        async Task<string> LoadSimilar()
        {

            Console.WriteLine(GlobalVars.Variables.currentID + " GlobalVars.Variables.currentID");
            var response = await BL.DataManager.GetSimilarJSON(GlobalVars.Variables.currentID);

            JSONObject lUserObj = new JSONObject(response);

            Models.RootSimilarObject subjects = JsonConvert.DeserializeObject<Models.RootSimilarObject>(response);

            Console.WriteLine(subjects);


            for (var i = 0; i < subjects.similar_movies.results.Count - 2; i++)
            {
                ls.Add(subjects.similar_movies.results[i].poster_path);
            };

            var gridview = FindViewById<GridView>(Resource.Id.gridview4);
            gridview.Adapter = new ImageAdapter(this, ls);

            gridview.ItemClick += (sender, args) =>
            {
                GlobalVars.Variables.currentID = subjects.similar_movies.results[args.Position].id.ToString();
                GlobalVars.Variables.currentPosterPath = subjects.similar_movies.results[args.Position].poster_path.ToString();
                GlobalVars.Variables.currentOriginalTitle = subjects.similar_movies.results[args.Position].original_title.ToString();
                GlobalVars.Variables.currentOverview = subjects.similar_movies.results[args.Position].overview.ToString();
                GlobalVars.Variables.currentReleaseDate = subjects.similar_movies.results[args.Position].release_date.ToString();

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
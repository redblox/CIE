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
using System.Net;
using Android.Graphics;
using Android.Content.PM;

namespace CIESample
{
    [Activity(Label = "SelectedMovie", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@android:style/Theme.NoTitleBar")]
    public class SelectedMovie : Activity
    {
        public List<string> ls = new List<string>();
        public ImageView MoviePoster;
        public Boolean isSaved = false;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SelectedMovie);

            TextView title = FindViewById<TextView>(Resource.Id.title_tv);
            title.Text = GlobalVars.Variables.currentOriginalTitle;

            TextView overview = FindViewById<TextView>(Resource.Id.overview_tv);
            overview.Text = GlobalVars.Variables.currentOverview;

            TextView ReleaseDate = FindViewById<TextView>(Resource.Id.release_tv);
            ReleaseDate.Text = "Release Date: " + GlobalVars.Variables.currentReleaseDate;

            Button saveBtn = FindViewById<Button>(Resource.Id.buttonSave);

            GetFilms();

            saveBtn.Click += SaveBtn_Click;

            MoviePoster = FindViewById<ImageView>(Resource.Id.Movie_iv);
            var imageBitmap = GetImageBitmapFromUrl("http://image.tmdb.org/t/p/w150" + GlobalVars.Variables.currentPosterPath);
            MoviePoster.SetImageBitmap(imageBitmap);

            var LoadSimilar_ = LoadSimilar();
        }

        private async void SaveBtn_Click(object sender, EventArgs e)
        {
            if (isSaved == true)
            {
                Toast.MakeText(this, "You already have this in your favorites.", ToastLength.Long).Show();
            }
            else
            {
                try
                {
                    FavoriteList.fa.Recreate();
                }
                catch (Exception ex)
                {

                }

                Toast.MakeText(this, "Saved to your favorites.", ToastLength.Long).Show();
                await DAL.Database.StroreFav(GlobalVars.Variables.currentOriginalTitle, GlobalVars.Variables.currentOverview, GlobalVars.Variables.currentPosterPath, GlobalVars.Variables.currentReleaseDate, GlobalVars.Variables.currentID);
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

        async Task<List<Models.Favorites>> GetFilms()
        {
            var FavFilms = await DAL.Database.GetFavFilms();

            for (var i = 0; i < FavFilms.Count; i++)
            {
                if (GlobalVars.Variables.currentID == FavFilms[i].Id.ToString())
                {
                    isSaved = true;
                    break;
                }
            };

            return null;
        }
    }
}
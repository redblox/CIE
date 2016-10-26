using System.Net;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System.Collections.Generic;
using System;

namespace CIESample
{
	public class ImageAdapter : BaseAdapter
	{
		Context context;
        List<string> passedItems;

		public ImageAdapter(Context c, List<string> stringPassed)
		{
			context = c;
            passedItems = stringPassed;

        }

		public override int Count
		{
			get { return passedItems.Count; }
		}

		public override Java.Lang.Object GetItem(int position)
		{
			return null;
		}

		public override long GetItemId(int position)
		{
			return 0;
		}

		// create a new ImageView for each item referenced by the Adapter
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			ImageView imageView;

			if (convertView == null)
			{  // if it's not recycled, initialize some attributes
				imageView = new ImageView(context);
				imageView.LayoutParameters = new GridView.LayoutParams(250, 350);
				imageView.SetScaleType(ImageView.ScaleType.CenterCrop);
				imageView.SetPadding(8, 8, 8, 8);
			}
			else {
				imageView = (ImageView)convertView;
			}
            
            var tempBtmp = "http://image.tmdb.org/t/p/w150";
            var passedString = tempBtmp + passedItems[position];
            
            if(passedString != "http://image.tmdb.org/t/p/w150")
            {
                Bitmap bitmap = GetBitmapFromUrl(passedString);
                imageView.SetImageBitmap(bitmap);
            }
            else
            {
                return imageView;
            }
			return imageView;
		}

		private Bitmap GetBitmapFromUrl(string url)
		{
			using (WebClient webClient = new WebClient())
			{
				byte[] bytes = webClient.DownloadData(url);
				if (bytes != null && bytes.Length > 0)
				{
					return BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);
				}
			}
			return null;
		}
	}
}





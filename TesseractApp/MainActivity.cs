using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Tesseract.Droid;
using System.Threading.Tasks;
using Android.Util;

namespace TesseractApp
{
	[Activity (Label = "TesseractApp", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			var CH = new CameraHelper();

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
			Button button1 = FindViewById<Button> (Resource.Id.myButton2);
			
			button.Click += delegate {

				var pkgManager = PackageManager;

				if(CH.AppToTakePicturesExists(pkgManager)){
					StartActivityForResult(CH.LaunchCameraApp(),0);
				}
				else{

					Toast.MakeText(
						Application.Context,
						"No Camera Application Found",
						ToastLength.Short).Show();
				}
			};

			// calling async method 1
			button1.Click += async (sender, e) => {

				Task<String> strResult = GetTextFromImage();
				var result = await strResult;
				Log.Info("DETECTED TEXT",result);
			Toast.MakeText (Application.Context,result,ToastLength.Long).Show();
			};

			// calling async method 2
			//button1.Click += GetTextFromImg;

		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);

			// Add the image to the photo gallery
			var mediaScanIntent = new Intent (Intent.ActionMediaScannerScanFile);
			Android.Net.Uri contentUri = Android.Net.Uri.FromFile (ImageInfo._file);
			mediaScanIntent.SetData (contentUri);
			SendBroadcast (mediaScanIntent);
		} 

		// async method 1
		private async Task<String> GetTextFromImage()
		{
			var tessAPI = new TesseractApi (Application.Context);
			await tessAPI.Init("eng");
			//tessAPI.SetWhitelist ("123456789");
			await tessAPI.SetImage (ImageInfo._file.ToString());
			return tessAPI.Text;
		}
			
		/* async method 2  using event handler
		 * The method is marked async but returns void. This is typically only done for event
		 * handlers otherwise you return a Task or Task<TResult>
		 */
		async void GetTextFromImg(object sender, EventArgs e)
		{
			var result = await GetTextFromImage ();
			Log.Info("DETECTED TEXT",result);
			Toast.MakeText (Application.Context,result,ToastLength.Long).Show();

		}

	}
}



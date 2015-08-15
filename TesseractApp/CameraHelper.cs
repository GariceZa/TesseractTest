using System;
using Android.Content.PM;
using Android.Content;
using Android.Provider;
using System.Collections.Generic;
using Android.Util;
using Java.IO;
using Android.Graphics;

namespace TesseractApp
{
	public class CameraHelper
	{
		public CameraHelper ()
		{
			CreatePictureDirectory ();
		}

		internal bool AppToTakePicturesExists(PackageManager pkgManager)
		{
			// Check if there is an application that can take pictures
			var intent = new Intent (MediaStore.ActionImageCapture);
			IList<ResolveInfo> availableActivities =  pkgManager.QueryIntentActivities (intent, PackageInfoFlags.MatchDefaultOnly);

			foreach (var item in availableActivities) {

				Log.Info ("AVAILABLE CAMERA APPS", item.ToString());
			}

			return availableActivities != null && availableActivities.Count > 0;
		}

		internal void CreatePictureDirectory()
		{
			// Set the phones DCIM/Camera directory
			ImageInfo._dir = new File (Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim),"Camera"); 

			if (!ImageInfo._dir.Exists ()) {
				ImageInfo._dir.Mkdirs ();
				Log.Info ("Directory Made",ImageInfo._dir.ToString());
			}

			Log.Info ("Directory",ImageInfo._dir.ToString());
		}

		internal Intent LaunchCameraApp()
		{
			// Create the intent to launch the camera application
			var intent = new Intent (MediaStore.ActionImageCapture);
			ImageInfo._file = new File (ImageInfo._dir, string.Format ("{0}.jpg", Guid.NewGuid ()));
			Log.Info ("FILE",ImageInfo._file.ToString());
			intent.PutExtra (MediaStore.ExtraOutput, Android.Net.Uri.FromFile(ImageInfo._file));
			return intent;
		}
	}

	public static class ImageInfo
	{
		internal static File _file { get; set;}
		internal static File _dir { get; set;}
		internal static Bitmap _bitmap { get; set;}
	}
}


﻿using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Provider;
using Android.Views;
using Android.Widget;
using Java.IO;

namespace FieldInspection
{

	using Environment = Android.OS.Environment;
	using Uri = Android.Net.Uri;

	public class InspectionFragment : Fragment
	{
		private ImageView _imageView;
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			//StartInspection();
			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);
			View view = inflater.Inflate(Resource.Layout.inspectionLayout, container, false);

			return view;

		}
		public override void OnStart()
		{
			base.OnStart();
			StartInspection();
		}

		void StartInspection()
		{
			if (IsThereAnAppToTakePictures())
			{
				CreateDirectoryForPictures();
				Button button =View.FindViewById<Button>(Resource.Id.myButton);
				_imageView = View.FindViewById<ImageView>(Resource.Id.imageView1);
				if (button != null && _imageView != null)
				{

					button.Click += TakeAPicture;
				}
			}
		}

		private void CreateDirectoryForPictures()
		{
			App._dir = new File(
				Environment.GetExternalStoragePublicDirectory(
					Environment.DirectoryPictures), "CameraAppDemo");
			if (!App._dir.Exists())
			{
				App._dir.Mkdirs();
			}
		}


		private void TakeAPicture(object sender, EventArgs eventArgs)
		{
			Intent intent = new Intent(MediaStore.ActionImageCapture);
			App._file = new File(App._dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
			intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(App._file));
			StartActivityForResult(intent, 0);
		}

		private bool IsThereAnAppToTakePictures()
		{
			Intent intent = new Intent(MediaStore.ActionImageCapture);
			IList<ResolveInfo> availableActivities = this.Activity
				.PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
			return availableActivities != null && availableActivities.Count > 0;
		}
	}
}

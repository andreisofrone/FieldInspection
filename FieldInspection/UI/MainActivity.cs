using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Widget;
using Java.IO;


namespace FieldInspection
{
    
    using Environment = Android.OS.Environment;
    using Uri = Android.Net.Uri;

    public static class App
    {
        public static File _file;
        public static File _dir;
        public static Bitmap bitmap;
    }
    [Activity(Label = "FieldInspection", Theme = "@style/MyTheme.Base", Icon = "@drawable/icon")]

	public class MainActivity : AppCompatActivity
	{
		DrawerLayout drawerLayout;
        private ImageView _imageView;
		private Fragment currentFragment;
		private DashboardFragment dashoardFragment;
		private InspectionFragment inspectionFragment;
		private Stack<Fragment> stackFragments;

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            // Make it available in the gallery

            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            Uri contentUri = Uri.FromFile(App._file);
            mediaScanIntent.SetData(contentUri);
            SendBroadcast(mediaScanIntent);

            // Display in ImageView. We will resize the bitmap to fit the display.
            // Loading the full sized image will consume to much memory
            // and cause the application to crash.

            int height = Resources.DisplayMetrics.HeightPixels;
            int width =  _imageView.Height;
            App.bitmap = App._file.Path.LoadAndResizeBitmap(width, height);
            if (App.bitmap != null)
            {
                _imageView.SetImageBitmap(App.bitmap);
                App.bitmap = null;
            }
            // Dispose of the Java side bitmap.
            GC.Collect();
        }

        protected override void OnCreate(Bundle savedInstanceState)
		{
			SetContentView(Resource.Layout.Main);	
			base.OnCreate(savedInstanceState);

			//dashoardFragment = new HomeFragment();
			////inspectionFragment = new InspectionFragment();
			//stackFragments = new Stack<Fragment>();



			//var trans = FragmentManager.BeginTransaction();
			//trans.Add(Resource.Id.HomeFrameLayout, dashoardFragment, "Dashoard Fragment");
			//trans.Hide(dashoardFragment);

			//trans.Add(Resource.Id.HomeFrameLayout, inspectionFragment, "Inspection Fragment");
			//trans.Commit();




			drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

			// Init toolbar
			var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_bar);
			SetSupportActionBar(toolbar);
			SupportActionBar.SetTitle(Resource.String.app_name);
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			SupportActionBar.SetDisplayShowHomeEnabled(true);


		
			// Create ActionBarDrawerToggle button and add it to the toolbar
			var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.open_drawer, Resource.String.close_drawer);

			if (drawerToggle != null)
			{
				drawerLayout.SetDrawerListener(drawerToggle);
			}

			drawerToggle.SyncState();

			//load default home screen
			var ft = FragmentManager.BeginTransaction();

			ft.AddToBackStack(null);
			ft.Add(Resource.Id.HomeFrameLayout, new DashboardFragment());
			ft.Commit();
			//currentFragment = dashoardFragment;

    //        if (IsThereAnAppToTakePictures())
    //        {
    //            CreateDirectoryForPictures();

    //            Button button = FindViewById<Button>(Resource.Id.myButton);
    //            _imageView = FindViewById<ImageView>(Resource.Id.imageView1);
				//if (button != null)
				//{

				//	button.Click += TakeAPicture;
				//}
    //        }
			var navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
			navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;
			// Attach item selected handler to navigation view

        }


		//private void ShowFragment(Fragment fragment)
		//{
		//	if (fragment.IsVisible)
		//	{
		//		return;
		//	}

		//	var trans = FragmentManager.BeginTransaction();
		//	//var a= supportf
		//	trans.SetCustomAnimations(Resource.Animation.slide_in, Resource.Animation.slide_out, Resource.Animation.slide_in, Resource.Animation.slide_out);

		//	fragment.View.BringToFront();
		//	currentFragment.View.BringToFront();

		//	trans.Hide(currentFragment);
		//	trans.Show(fragment);

		//	trans.AddToBackStack(null);
		//	stackFragments.Push(currentFragment);
		//	trans.Commit();

		//	currentFragment = fragment;
		//}


        private void TakeAPicture(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            App._file = new File(App._dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
            intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(App._file));
            StartActivityForResult(intent, 0);
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

        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

		//define custom title text
		protected override void OnResume()
		{
			SupportActionBar.SetTitle(Resource.String.app_name);
			base.OnResume();
		}
		//define action for navigation menu selection
		void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
		{
			switch (e.MenuItem.ItemId)
			{
				case (Resource.Id.nav_dashboard):
					
					var ft = FragmentManager.BeginTransaction();
					var home = new DashboardFragment();
					var insp = new InspectionFragment();

					//insp.View.BringToFront();
					//home.View.BringToFront();

					//ft.Hide(insp);
					//ft.Show(home);

					ft.AddToBackStack(null);
					ft.Add(Resource.Id.HomeFrameLayout, home);

					ft.Commit();

					break;
					
				case (Resource.Id.nav_inspection):
					
					var ftt = FragmentManager.BeginTransaction();
					var homee = new DashboardFragment();
					var inspp = new InspectionFragment();

					//insp.View.BringToFront();
					//home.View.BringToFront();

					//ftt.Hide(homee);
					//ftt.Show(inspp);

					ftt.AddToBackStack(null);
					ftt.Add(Resource.Id.HomeFrameLayout, inspp);

					ftt.Commit();

						break;

					//case (Resource.Id.nav_friends):
					//	// React on 'Friends' selection
					//	break;
			}
			// Close drawer
			drawerLayout.CloseDrawers();
		}

		//add custom icon to tolbar
		public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.action_menu, menu);
			if (menu != null)
			{
				menu.FindItem(Resource.Id.action_refresh).SetVisible(true);
				menu.FindItem(Resource.Id.action_attach).SetVisible(false);
			}
			return base.OnCreateOptionsMenu(menu);
		}

		//define action for tolbar icon press
		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Android.Resource.Id.Home:
					//this.Activity.Finish();
					return true;
				case Resource.Id.action_attach:
					//FnAttachImage();
					return true;
				default:
					return base.OnOptionsItemSelected(item);
			}
		}
		//to avoid direct app exit on backpreesed and to show fragment from stack
		public override void OnBackPressed()
		{
			if (FragmentManager.BackStackEntryCount != 0)
			{
				FragmentManager.PopBackStack();// fragmentManager.popBackStack();
			}
			else {
				base.OnBackPressed();
			}
		}
	}
}


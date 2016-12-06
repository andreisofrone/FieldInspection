using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.App;
using System.Threading.Tasks;
using System.Net;
using System;
using System.IO;
using System.Json;
using Xamarin.Android;


namespace FieldInspection
{
	[Activity(Label = "FieldInspection", Theme = "@style/MyTheme.Base", Icon = "@drawable/icon")]
	public class MainActivity : AppCompatActivity
	{
		DrawerLayout drawerLayout;
		protected override void OnCreate(Bundle savedInstanceState)
		{


			SetContentView(Resource.Layout.Main);	
			base.OnCreate(savedInstanceState);

			drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

			// Init toolbar
			var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_bar);
			SetSupportActionBar(toolbar);
			SupportActionBar.SetTitle(Resource.String.app_name);
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			SupportActionBar.SetDisplayShowHomeEnabled(true);

			// Attach item selected handler to navigation view
			var navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
			navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;

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
			ft.Add(Resource.Id.HomeFrameLayout, new HomeFragment());
			ft.Commit();
		}

		private async Task<JsonValue> FetchWeatherAsync(string url)
		{
			// Create an HTTP web request using the URL:
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
			request.ContentType = "application/json";
			request.Method = "GET";

			// Send the request to the server and wait for the response:
			using (WebResponse response = await request.GetResponseAsync())
			{
				// Get a stream representation of the HTTP web response:
				using (Stream stream = response.GetResponseStream())
				{
					// Use this stream to build a JSON document object:
					JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
					Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());

					// Return the JSON document:
					return jsonDoc;
				}
			}
		}
		private void ParseAndDisplay(JsonValue json)
		{
			// Get the weather reporting fields from the layout resource:
			var location = "";
			var temperature = "";
			var humidity = "";
			var conditions = "";

			// Extract the array of name/value results for the field name "weatherObservation". 
			JsonValue weatherResults = json["status"];

			// Extract the "stationName" (location string) and write it to the location TextBox:
			location = weatherResults["stationName"];

			// The temperature is expressed in Celsius:
			double temp = weatherResults["temperature"];
			// Convert it to Fahrenheit:
			temp = ((9.0 / 5.0) * temp) + 32;
			// Write the temperature (one decimal place) to the temperature TextBox:
			temperature = String.Format("{0:F1}", temp) + "° F";

			// Get the percent humidity and write it to the humidity TextBox:
			double humidPercent = weatherResults["humidity"];
			humidity = humidPercent.ToString() + "%";

			// Get the "clouds" and "weatherConditions" strings and 
			// combine them. Ignore strings that are reported as "n/a":
			string cloudy = weatherResults["clouds"];
			if (cloudy.Equals("n/a"))
				cloudy = "";
			string cond = weatherResults["weatherCondition"];
			if (cond.Equals("n/a"))
				cond = "";

			// Write the result to the conditions TextBox:
			conditions = cloudy + " " + cond;

			Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
			alert.SetTitle(location);
			alert.SetMessage(temperature);
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
					Toast.MakeText(this, "Dashboard selected!", ToastLength.Short).Show();
					break;
				case (Resource.Id.nav_inspection):
						Toast.MakeText(this, "Inspection selected!", ToastLength.Short).Show();
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


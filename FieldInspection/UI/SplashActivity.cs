using Android.App;
using Android.OS;

namespace FieldInspection
{
	[Activity(Theme = "@style/Theme.Splash", MainLauncher = true, Icon = "@drawable/icon")]
	public class SplashActivity : Activity
	{
		System.Timers.Timer timer;

		protected override void OnCreate(Bundle bundle)
		{

			base.OnCreate(bundle);
			SetContentView(Resource.Layout.SplashLayout);
			timer = new System.Timers.Timer();
			timer.Interval = 2500;
			timer.Elapsed += t_Elapsed;
			timer.Start();
		}

		void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			timer.Stop();
			StartActivity(typeof(LoginActivity));
			Finish();
		}
	}
}


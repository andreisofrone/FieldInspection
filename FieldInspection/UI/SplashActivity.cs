using System;
using System.Threading;
using Android.App;
using Android.OS;

namespace FieldInspection
{
	[Activity(Theme = "@style/Theme.Splash", NoHistory = true)]
	public class SplashActivity : Activity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			Thread.Sleep(2500);
			StartActivity(typeof(MainActivity));
		}
	}
}


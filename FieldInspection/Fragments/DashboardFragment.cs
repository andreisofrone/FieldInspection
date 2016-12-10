using Android.App;
using Android.OS;
using Android.Views;

namespace FieldInspection
{
	public class DashboardFragment : Fragment
	{
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);
			View view = inflater.Inflate(Resource.Layout.dashboardLayout, container, false);
			return view;//base.OnCreateView (inflater.Inflate(Resource.Layout.homeLayout, container, savedInstanceState);
		}
	}
}
using Android.App;
using Android.OS;
using Android.Support.V7.App;

namespace FieldInspection
{
    [Activity(Label = "Dashboard", Theme = "@style/MyTheme.Base", Icon = "@drawable/icon")]
    public class DashboardActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            // Create your application here
        }
    }
}
using Android.App;
using Android.Content.PM;
using Android.OS;

namespace TodoApp
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Zwingt Android 14+, den Inhalt nicht hinter die Systemleisten clippen zu lassen
            Window.SetDecorFitsSystemWindows(false);
        }
    }
}

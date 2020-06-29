using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.Messaging;
using Android;

namespace AppHelp.Droid
{
    [Activity(Label = "AppHelp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        const int RequestLocationId = 0;

        readonly string[] Permissions =
        {
            Manifest.Permission.SendSms,
            Manifest.Permission.CallPhone,
            Manifest.Permission.AccessFineLocation,
            Manifest.Permission.AccessCoarseLocation
        };
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            CrossMessaging.Current.Settings().Phone.AutoDial = true;

            if (NeedPermissions(this))
            {
                RequestPermissions();
            }

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        public static bool NeedPermissions(Activity activity)
        {
            return activity.CheckSelfPermission(Manifest.Permission.Camera) != Permission.Granted ||
            activity.CheckSelfPermission(Manifest.Permission.WriteExternalStorage) != Permission.Granted;
        }

        void RequestPermissions()
        {
            RequestPermissions(Permissions, RequestLocationId);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
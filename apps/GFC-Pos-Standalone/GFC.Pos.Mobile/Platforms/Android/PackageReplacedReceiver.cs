using Android.App;
using Android.Content;
using Android.Widget;

namespace GFC.Pos.Mobile.Platforms.Android
{
    [BroadcastReceiver(Name = "com.gfc.pos.mobile.PackageReplacedReceiver", Enabled = true, Exported = true)]
    [IntentFilter(new[] { Intent.ActionMyPackageReplaced })]
    public class PackageReplacedReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent?.Action == Intent.ActionMyPackageReplaced)
            {
                // Retrieve the launch intent for MainActivity
                Intent? launchIntent = context.PackageManager?.GetLaunchIntentForPackage(context.PackageName);
                if (launchIntent != null)
                {
                    launchIntent.AddFlags(ActivityFlags.NewTask);
                    launchIntent.AddFlags(ActivityFlags.ClearTop);
                    launchIntent.AddFlags(ActivityFlags.SingleTop);
                    launchIntent.PutExtra("LaunchedFromUpdate", true);

                    context.StartActivity(launchIntent);
                }
            }
        }
    }
}

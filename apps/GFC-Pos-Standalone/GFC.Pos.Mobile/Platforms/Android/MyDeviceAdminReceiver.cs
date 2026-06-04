using Android.App;
using Android.App.Admin;
using Android.Content;
using Android.Widget;

namespace GFC.Pos.Mobile.Platforms.Android
{
    [BroadcastReceiver(Name = "com.gfc.pos.mobile.MyDeviceAdminReceiver", Permission = "android.permission.BIND_DEVICE_ADMIN", Exported = true)]
    [IntentFilter(new[] { DeviceAdminReceiver.ActionDeviceAdminEnabled })]
    [MetaData("android.app.device_admin", Resource = "@xml/device_admin_policies")]
    public class MyDeviceAdminReceiver : DeviceAdminReceiver
    {
        public override void OnEnabled(Context context, Intent intent)
        {
            base.OnEnabled(context, intent);
            Toast.MakeText(context, "POS MDM Admin Enabled", ToastLength.Short)?.Show();
        }

        public override void OnDisabled(Context context, Intent intent)
        {
            base.OnDisabled(context, intent);
            Toast.MakeText(context, "POS MDM Admin Disabled", ToastLength.Short)?.Show();
        }
    }
}

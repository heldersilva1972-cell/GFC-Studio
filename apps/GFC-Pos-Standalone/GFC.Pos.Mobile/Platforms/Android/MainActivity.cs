using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace GFC.Pos.Mobile;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
[IntentFilter(new[] { "android.hardware.usb.action.USB_DEVICE_ATTACHED" })]
[MetaData("android.hardware.usb.action.USB_DEVICE_ATTACHED", Resource = "@xml/device_filter")]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        
        // Initial call
        SetWindowLayout();
    }

    public override void OnWindowFocusChanged(bool hasFocus)
    {
        base.OnWindowFocusChanged(hasFocus);
        if (hasFocus)
        {
            // Many Android terminals override system UI flags during the initial focus event.
            // A small delay ensures our 'Hide' command is the final word.
            new Handler(Looper.MainLooper).PostDelayed(SetWindowLayout, 500);
        }
    }

    private void SetWindowLayout()
    {
        if (Window == null) return;

        // 1. Edge-to-Edge: Tell the OS the app handles its own insets
        // This prevents the OS from 'pushing' the webview up to make room for the bar.
        Window.SetDecorFitsSystemWindows(false);

        // 2. Transparency Fallback: If the bar does flicker back, make it invisible
        Window.SetNavigationBarColor(Android.Graphics.Color.Transparent);
        Window.SetStatusBarColor(Android.Graphics.Color.Transparent);

        if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
        {
            // Modern API (Android 11 / API 30+)
            var controller = Window.InsetsController;
            if (controller != null)
            {
                // Hide both Status and Navigation bars
                controller.Hide(WindowInsets.Type.StatusBars() | WindowInsets.Type.NavigationBars());
                
                // Sticky Behavior: Requires a swipe to show, and auto-hides again
                controller.SystemBarsBehavior = (int)WindowInsetsControllerBehavior.ShowTransientBarsBySwipe;
            }
        }
        else
        {
            // Legacy API (Android 10 and below)
            #pragma warning disable CS0618
            var uiOptions = (int)Window.DecorView.SystemUiVisibility;

            uiOptions |= (int)SystemUiFlags.LowProfile;
            uiOptions |= (int)SystemUiFlags.Fullscreen;
            uiOptions |= (int)SystemUiFlags.HideNavigation;
            uiOptions |= (int)SystemUiFlags.ImmersiveSticky;

            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;
            #pragma warning restore CS0618
        }
    }
}

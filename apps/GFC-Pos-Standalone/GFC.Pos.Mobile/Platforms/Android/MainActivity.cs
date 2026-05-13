using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace GFC.Pos.Mobile;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
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

        // Ensure the layout can expand into the system areas
        Window.SetDecorFitsSystemWindows(false);

        if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
        {
            // Modern API (Android 11 / API 30+)
            var controller = Window.InsetsController;
            if (controller != null)
            {
                controller.Hide(WindowInsets.Type.StatusBars() | WindowInsets.Type.NavigationBars());
                controller.SystemBarsBehavior = (int)WindowInsetsControllerBehavior.ShowTransientBarsBySwipe;
            }
        }
        else
        {
            // Legacy API (Android 10 and below)
            #pragma warning disable CS0618 // Type or member is obsolete
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

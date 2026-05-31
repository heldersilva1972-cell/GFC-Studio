namespace GFC.Pos.Mobile;

public partial class App : Application
{
	public App()
	{
		System.Diagnostics.Debug.WriteLine("[App] Constructor started.");
		InitializeComponent();
		System.Diagnostics.Debug.WriteLine("[App] InitializeComponent completed.");
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		System.Diagnostics.Debug.WriteLine("[App] CreateWindow called.");
		return new Window(new MainPage()) { Title = "GFC.Pos.Mobile" };
	}
}

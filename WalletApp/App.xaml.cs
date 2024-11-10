using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;

namespace WalletApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }

#if WINDOWS
    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);


        window.Width = 600;
        window.Height = 1200;


        window.X = 100;
        window.Y = 100;

        return window;
    }
#endif
    
    protected override async void OnStart()
    {
        var pin = await SecureStorage.GetAsync("user_pin");
        
        if (!string.IsNullOrEmpty(pin))
        {
            await Shell.Current.GoToAsync("//PinEntryPage");
        }
        else
        {
            await Shell.Current.GoToAsync("//SetPinPage");
        }
    }
}
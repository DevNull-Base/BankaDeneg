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
    
    protected override async void OnStart()
    {
        var pin = await SecureStorage.GetAsync("user_pin");
        
        /*if (!string.IsNullOrEmpty(pin))
        {
            await Shell.Current.GoToAsync("//PinEntryPage");
        }
        else
        {
            await Shell.Current.GoToAsync("//SetPinPage");
        }*/
    }
}
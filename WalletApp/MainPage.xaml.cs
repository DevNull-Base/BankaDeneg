using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;

namespace WalletApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        this.Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Colors.Transparent,
            StatusBarStyle = StatusBarStyle.LightContent
        });
    }
}
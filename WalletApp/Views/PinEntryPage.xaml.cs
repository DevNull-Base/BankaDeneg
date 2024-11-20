using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using WalletApp.ViewModels;

namespace WalletApp.Views;

public partial class PinEntryPage : ContentPage
{
    public PinEntryPage()
    {
        InitializeComponent();
        
        this.Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Colors.Transparent,
            StatusBarStyle = StatusBarStyle.LightContent
        });
        
        BindingContext = new PinEntryViewModel();
    }
}
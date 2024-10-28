using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using WalletApp.ViewModels;

namespace WalletApp.Views;

public partial class SetPinPage : ContentPage
{
    public SetPinPage()
    {
        InitializeComponent();
        
        this.Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Colors.Transparent,
            StatusBarStyle = StatusBarStyle.LightContent
        });
        
        BindingContext = new SetPinViewModel();
    }
}
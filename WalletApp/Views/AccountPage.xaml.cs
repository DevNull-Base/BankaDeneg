using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using WalletApp.ViewModels;

namespace WalletApp.Views;

public partial class AccountPage : ContentPage
{
    public AccountPage()
    {
        InitializeComponent();
        var item = new StatusBarBehavior();
        item.StatusBarStyle = StatusBarStyle.LightContent;
        this.Behaviors.Add(item);

        BindingContext = new AccountViewModel();
    }
}
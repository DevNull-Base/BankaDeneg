using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;

namespace WalletApp.Views;

public partial class ContactsPage : ContentPage
{
    public ContactsPage()
    {
        InitializeComponent();
        var item = new StatusBarBehavior();
        item.StatusBarStyle = StatusBarStyle.LightContent;
        item.StatusBarColor = Colors.Transparent;
        this.Behaviors.Add(item);
    }
    
    protected override bool OnBackButtonPressed()
    {
        GoBack();
        return true;
    }
    
    private async void GoBack()
    {
        await Shell.Current.GoToAsync("//Payment");
    }

    private void GoBack(object? sender, EventArgs e)
    {
        GoBack();
    }
}
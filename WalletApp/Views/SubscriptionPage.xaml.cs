using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.ViewModels;

namespace WalletApp.Views;

public partial class SubscriptionPage : ContentPage
{
    public SubscriptionPage()
    {
        InitializeComponent();
        BindingContext = new SubscriptionViewModel();
    }
    
    protected override bool OnBackButtonPressed()
    {
        GoBack();
        return true;
    }
    
    private async void GoBack()
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    private void GoBack(object? sender, EventArgs e)
    {
        GoBack();
    }
}
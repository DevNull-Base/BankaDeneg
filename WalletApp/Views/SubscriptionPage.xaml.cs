using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Views;

public partial class SubscriptionPage : ContentPage
{
    public SubscriptionPage()
    {
        InitializeComponent();
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
}
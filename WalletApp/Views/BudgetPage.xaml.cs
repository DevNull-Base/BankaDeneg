using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using WalletApp.ViewModels;
using WalletApp.Views.PopupViews;

namespace WalletApp.Views;

public partial class BudgetPage : ContentPage
{
    public BudgetPage(BudgetViewModel budgetViewModel)
    {
        InitializeComponent();
        var item = new StatusBarBehavior();
        item.StatusBarStyle = StatusBarStyle.LightContent;
        item.StatusBarColor = Colors.Transparent;
        this.Behaviors.Add(item);

        BindingContext = budgetViewModel;
    }
    
    protected override bool OnBackButtonPressed()
    {
        ShowConfirmationPopup();
        return true;
    }
    
    private async void ShowConfirmationPopup()
    {
        var popup = new ExitPopupView();
        var result = await this.ShowPopupAsync(popup);

        if (result is bool boolResult && boolResult)
        {
            App.Current.Quit();
        }
    }
}
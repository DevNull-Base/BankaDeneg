﻿using System;
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

public partial class AccountPage : ContentPage
{
    public AccountPage(AccountViewModel accountViewModel)
    {
        InitializeComponent();
        var item = new StatusBarBehavior();
        item.StatusBarStyle = StatusBarStyle.DarkContent;
        this.Behaviors.Add(item);

        BindingContext = accountViewModel;
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
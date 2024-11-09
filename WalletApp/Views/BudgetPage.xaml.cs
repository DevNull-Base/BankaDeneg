using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using WalletApp.ViewModels;

namespace WalletApp.Views;

public partial class BudgetPage : ContentPage
{
    public BudgetPage()
    {
        InitializeComponent();
        var item = new StatusBarBehavior();
        item.StatusBarStyle = StatusBarStyle.LightContent;
        item.StatusBarColor = Colors.Transparent;
        this.Behaviors.Add(item);

        BindingContext = new BudgetViewModel();
    }
}
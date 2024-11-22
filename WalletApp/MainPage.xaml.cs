using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using WalletApp.ViewModels;
using WalletApp.Views.PopupViews;

namespace WalletApp;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        this.Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Colors.Transparent,
            StatusBarStyle = StatusBarStyle.LightContent
        });

        BindingContext = viewModel;

        if ( viewModel.Categories.Count == 0)
        {
            return;
        }
        
        var totalValue = viewModel.Categories.Sum(c => c.Value);

        foreach (var category in viewModel.Categories)
        {
            var columnWidth = category.Value / totalValue;
            ProgressBarGrid.ColumnDefinitions.Add(new ColumnDefinition
                { Width = new GridLength(columnWidth, GridUnitType.Star) });

            var boxView = new BoxView
            {
                Color = category.Color,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            Grid.SetColumn(boxView, ProgressBarGrid.ColumnDefinitions.Count - 1);
            ProgressBarGrid.Children.Add(boxView);
        }
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
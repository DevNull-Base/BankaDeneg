using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using WalletApp.ViewModels;

namespace WalletApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        this.Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Colors.Transparent,
            StatusBarStyle = StatusBarStyle.LightContent
        });

        var viewModel = new MainViewModel();

        BindingContext = viewModel;
        
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
}
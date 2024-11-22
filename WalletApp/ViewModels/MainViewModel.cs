using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SharedModels;
using WalletApp.Models;
using WalletApp.Services;
using WalletApp.ViewModels.PopupModelView;
using WalletApp.Views.PopupViews;
using Transaction = WalletApp.Models.Transaction;

namespace WalletApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IDataService _dataService;
    private readonly NavigationService _navigationService;
    private readonly IDispatcher _dispatcher;

    private ObservableCollection<Item> _items;
    private ObservableCollection<Transaction> _transactions;

    public ObservableCollection<Item> Items
    {
        get => _items;
        set => SetProperty(ref _items, value);
    }

    public ObservableCollection<Transaction> Transactions
    {
        get => _transactions;
        set => SetProperty(ref _transactions, value);
    }

    public double ScreenWidth { get; private set; }
    public double ScreenWidthAdd25 { get; private set; }
    public List<Category> Categories { get; set; }

    public IRelayCommand<string> NavigateToPage { get; }
    static Page MainPage => Shell.Current;

    public MainViewModel(IDataService dataService)
    {
        _dispatcher = Application.Current?.Dispatcher;

        Items = new ObservableCollection<Item>();
        Transactions = new ObservableCollection<Transaction>();

        ScreenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
        ScreenWidthAdd25 = ScreenWidth + 100;


        _dataService = dataService;

        _navigationService = new NavigationService();

        NavigateToPage = new RelayCommand<string>(_navigationService.NavigateToPageAsync);


        Categories = new List<Category>
        {
            new Category { Name = "Food", Value = 55, Color = Color.FromArgb("#6DCA68") },
            new Category { Name = "Transport", Value = 15, Color = Color.FromArgb("#FD3D3D") },
            new Category { Name = "Entertainment", Value = 12, Color = Color.FromArgb("#AB297B") },
            new Category { Name = "Utilities", Value = 12, Color = Color.FromArgb("#3A79FF") },
            new Category { Name = "Other", Value = 6, Color = Color.FromArgb("#CECECE") },
        };

        InitializeAsync();
    }

    private void InitializeAsync()
    {
         LoadItemsAsync();
         LoadTransactionsAsync();
    }


    private async void LoadItemsAsync()
    {
        var tmp = await _dataService.GetItemData();
        _dispatcher?.Dispatch(() => { Items = new ObservableCollection<Item>(tmp); });
    }
    
    private async void LoadTransactionsAsync()
    {
        var tmp2 = await _dataService.GetTransactionData();
        _dispatcher?.Dispatch(() => { Transactions = new ObservableCollection<Transaction>(tmp2); });
    }

    [RelayCommand]
    private void OpenCardPopup()
    {
        var p = new CardholderPopupView();
        MainPage.ShowPopup(p);
    }

    [RelayCommand]
    private async Task GoSubscriptions()
    {
        await Shell.Current.GoToAsync("//Subscriptions");
    }
}
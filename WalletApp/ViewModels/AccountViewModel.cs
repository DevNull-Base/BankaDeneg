using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WalletApp.Models;
using WalletApp.Services;
using WalletApp.Views.PopupViews;

namespace WalletApp.ViewModels;

public partial class AccountViewModel : ObservableObject
{
    private readonly IDataService _dataService;
    
    public IRelayCommand<string> NavigateToPage { get; }

    private readonly NavigationService _navigationService;
    private readonly IDispatcher _dispatcher;


    private ObservableCollection<Item> _items;

    public ObservableCollection<Item> Items
    {
        get => _items;
        set => SetProperty(ref _items, value);
    }
    
    static Page MainPage => Shell.Current;

    public AccountViewModel(IDataService dataService)
    {
        _dispatcher = Application.Current?.Dispatcher;
        Items = new ObservableCollection<Item>();

        _dataService = dataService;
        _navigationService = new NavigationService();

        NavigateToPage = new RelayCommand<string>(_navigationService.NavigateToPageAsync);

        InitializeAsync();
    }

    private void InitializeAsync()
    {
        LoadItemsAsync();
    }


    private async void LoadItemsAsync()
    {
        var tmp = await _dataService.GetItemData();
        _dispatcher?.Dispatch(() => { Items = new ObservableCollection<Item>(tmp); });
    }
    
    
    [RelayCommand]
    private void OpenCardPopup(Item i)
    {
        var p = new CardholderPopupView();
        MainPage.ShowPopup(p);
    }
}
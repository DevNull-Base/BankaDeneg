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
    private readonly IAuthService _authService;
    private readonly IAPIService _apiService;
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

    public AccountViewModel()
    {
        _dispatcher = Application.Current?.Dispatcher;
        Items = new ObservableCollection<Item>();

        _authService = new AuthService();
        _apiService = new APIService(_authService);
        _navigationService = new NavigationService();

        NavigateToPage = new RelayCommand<string>(_navigationService.NavigateToPageAsync);

        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        await LoginAsync();
    }


    private async Task LoginAsync()
    {
        try
        {
            var success = await _authService.IsAuthenticatedAsync();
            if (!success) throw new UnauthorizedAccessException();

            var accounts = await _apiService.GetAccountsAsync();

            decimal totalAmount = 0;

            foreach (var account in accounts)
            {
                if (decimal.TryParse(account.Amount, NumberStyles.Any, CultureInfo.InvariantCulture,
                        out decimal amount))
                {
                    totalAmount += amount;
                }
            }

            var tmp = new ObservableCollection<Item>
            {
                new Item
                {
                    Title = "Общий счет",
                    Description = totalAmount + " ₽",
                    Shape = "form_card.svg"
                }
            };

            foreach (var ac in accounts)
            {
                var item = new Item
                {
                    Title = ac.BankName,
                    Description = ac.Amount + " ₽"
                };
                

                if (ac.BankName[0] == 'В')
                {
                    item.Shape = "form_card_blue.svg";
                }
                else if (ac.BankName[0] == 'С')
                {
                    item.Shape = "form_card_green.svg";
                }
                else
                {
                    item.Shape = "form_card_orange.svg";
                }
                
                tmp.Add(item);
            }

            _dispatcher?.Dispatch(() => { Items = new ObservableCollection<Item>(tmp); });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            _dispatcher?.Dispatch(() =>
            {
                Items = new ObservableCollection<Item>
                {
                    new Item { Title = "Общий счет", Description = "123 030\u20bd", Shape = "form_card.svg" },
                    new Item { Title = "ВТБ", Description = "23 030\u20bd", Shape = "form_card_blue.svg" },
                    new Item { Title = "СБЕР", Description = "50 000\u20bd", Shape = "form_card_green.svg" },
                    new Item { Title = "Т БАНК", Description = "50 000\u20bd", Shape = "form_card_orange.svg" },
                };

            });
        }
    }
    
    [RelayCommand]
    private void OpenCardPopup(Item i)
    {
        var p = new CardholderPopupView();
        MainPage.ShowPopup(p);
    }
}
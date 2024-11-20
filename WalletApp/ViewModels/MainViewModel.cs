using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SharedModels;
using WalletApp.Models;
using WalletApp.Services;
using Transaction = WalletApp.Models.Transaction;

namespace WalletApp.ViewModels;

public partial class MainViewModel : ObservableObject, INotifyPropertyChanged
{
    private readonly IAuthService _authService;
    private readonly IAPIService _apiService;
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

    public MainViewModel()
    {
        _dispatcher = Application.Current?.Dispatcher;

        Items = new ObservableCollection<Item>();
        Transactions = new ObservableCollection<Transaction>();

        ScreenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
        ScreenWidthAdd25 = ScreenWidth + 100;

        _authService = new AuthService();
        _apiService = new APIService(_authService);

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

    private async void InitializeAsync()
    {
        await LoginAsync();
    }


    private async Task LoginAsync()
    {
        try
        {
            var credentials = ReadCredentialsFromJson();
            var login = credentials.Login;
            var password = credentials.Password;

            var success = await _authService.AuthenticateAsync(login, password);
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


            var tmp2 = new ObservableCollection<Transaction>();

            var transactions = await _apiService.GetTransactionsAsync();

            foreach (var t in transactions)
            {
                tmp2.Add(new Transaction
                {
                    Description = "Бургер Кинг",
                    Category = t.Type,
                    Amount = "-" + t.Amount + " ₽",
                    Source = t.BankName,
                });
            }

            _dispatcher?.Dispatch(() => { Transactions = new ObservableCollection<Transaction>(tmp2); });
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

                Transactions = new ObservableCollection<Transaction>
                {
                    new Transaction
                        { Description = "Бургер Кинг", Category = "Фастфуд", Amount = "-299.97₽", Source = "ВТБ" },
                    new Transaction
                        { Description = "Николай Ж.", Category = "Переводы", Amount = "-299.97₽", Source = "СБЕР" }
                };
            });
        }
    }

    private (string Login, string Password) ReadCredentialsFromJson()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "WalletApp.credentials.json";

        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        using (StreamReader reader = new StreamReader(stream))
        {
            string result = reader.ReadToEnd();
            var credentials = JsonSerializer.Deserialize<Credentials>(result);
            return (credentials.Login, credentials.Password);
        }
    }
    
    [RelayCommand]
    private async Task GoSubscriptions()
    {
        await Shell.Current.GoToAsync("//Subscriptions");
    }

    private class Credentials
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
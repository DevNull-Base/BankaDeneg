using System.Collections.ObjectModel;
using System.Globalization;
using WalletApp.Models;

namespace WalletApp.Services;

public interface IDataService
{
    public Task<ObservableCollection<Item>> GetItemData();
    public Task<ObservableCollection<Transaction>> GetTransactionData();
}

public class DataService : IDataService
{
    private readonly IAPIService _apiService;
    
    private ObservableCollection<Item> _items = new ObservableCollection<Item>();
    private ObservableCollection<Transaction> _transactions = new ObservableCollection<Transaction>();

    public DataService()
    {
        var authService = new AuthService();
        _apiService = new APIService(authService);
        
        LoadItemData();
        LoadTransactionData();
    }
    

    public async void LoadItemData()
    {
        var tmp = new ObservableCollection<Item>();

        try
        {
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

            tmp = new ObservableCollection<Item>
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

            _items = new ObservableCollection<Item>(tmp);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            _items = new ObservableCollection<Item>
            {
                new Item { Title = "Общий счет", Description = "123 030\u20bd", Shape = "form_card.svg" },
                new Item { Title = "ВТБ", Description = "23 030\u20bd", Shape = "form_card_blue.svg" },
                new Item { Title = "СБЕР", Description = "50 000\u20bd", Shape = "form_card_green.svg" },
                new Item { Title = "Т БАНК", Description = "50 000\u20bd", Shape = "form_card_orange.svg" },
            };
            
        }
    }

    public async void LoadTransactionData()
    {
        var tmp = new ObservableCollection<Transaction>();

        try
        {
            _transactions = new ObservableCollection<Transaction>();

            var transactions = await _apiService.GetTransactionsAsync();

            foreach (var t in transactions)
            {
                tmp.Add(new Transaction
                {
                    Description = "Бургер Кинг",
                    Category = t.Type,
                    Amount = "-" + t.Amount + " ₽",
                    Source = t.BankName,
                });
            }

            _transactions = new ObservableCollection<Transaction>(tmp);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            _transactions = new ObservableCollection<Transaction>
            {
                new Transaction
                    { Description = "Бургер Кинг", Category = "Фастфуд", Amount = "-299.97₽", Source = "ВТБ" },
                new Transaction
                    { Description = "Николай Ж.", Category = "Переводы", Amount = "-299.97₽", Source = "СБЕР" }
            };
            
        }
    }

    public async Task<ObservableCollection<Item>> GetItemData()
    {
        while (_items.Count == 0)
        {
            await Task.Delay(1000);
        }
        return _items;
    }

    public async Task<ObservableCollection<Transaction>> GetTransactionData()
    {
        while (_transactions.Count == 0)
        {
            await Task.Delay(1000);
        }
        return _transactions;
    }
}
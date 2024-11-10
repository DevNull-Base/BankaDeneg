using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WalletApp.Models;
using WalletApp.Services;

namespace WalletApp.ViewModels;

public partial class BudgetViewModel : ObservableObject
{
    private readonly IAuthService _authService;
    private readonly IAPIService _apiService;
    public IRelayCommand<string> NavigateToPage { get; }

    private readonly NavigationService _navigationService;

    private bool _isExpensesSelected = true;
    private readonly IDispatcher _dispatcher;
    
    private ObservableCollection<Transaction> _transactions;
    
    public ObservableCollection<Transaction> Transactions
    {
        get => _transactions;
        set => SetProperty(ref _transactions, value);
    }

    public BudgetViewModel()
    {
        _dispatcher = Application.Current?.Dispatcher;
        
        _authService = new AuthService();
        _apiService = new APIService(_authService);
        _navigationService = new NavigationService();
        
        NavigateToPage = new RelayCommand<string>(_navigationService.NavigateToPageAsync);
        
        InitializeAsync();
    }


    public bool IsExpensesSelected
    {
        get => _isExpensesSelected;
        set
        {
            SetProperty(ref _isExpensesSelected, value);
            OnPropertyChanged(nameof(IsIncomesSelected));
        }
    }

    public bool IsIncomesSelected => !IsExpensesSelected;

    [RelayCommand]
    private void ToggleExpenses()
    {
        IsExpensesSelected = true;
    }

    [RelayCommand]
    private void ToggleIncomes()
    {
        IsExpensesSelected = false;
    }
    
     private async void InitializeAsync()
    {
        await LoginAsync();
    }


    private async Task LoginAsync()
    {

        var success = await _authService.IsAuthenticatedAsync();
        if (success)
        {
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
            
            _dispatcher?.Dispatch(() =>
            {
                Transactions = new ObservableCollection<Transaction>(tmp2);
            });
            
        }
        else
        {
            _dispatcher?.Dispatch(() =>
            {
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
}
using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WalletApp.Models;
using WalletApp.Services;

namespace WalletApp.ViewModels;

public partial class BudgetViewModel : ObservableObject
{
    private IDataService _dataService;
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

    public BudgetViewModel(IDataService dataService)
    {
        _dispatcher = Application.Current?.Dispatcher;

        _dataService = dataService;

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
        var tmp2 = await _dataService.GetTransactionData();

        _dispatcher?.Dispatch(() => { Transactions = new ObservableCollection<Transaction>(tmp2); });
    }
}
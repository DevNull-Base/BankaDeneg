using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WalletApp.Models;
using WalletApp.Services;

namespace WalletApp.ViewModels;

public partial class BudgetViewModel : ObservableObject
{
    public ObservableCollection<Transaction> Transactions { get; set; }

    public IRelayCommand<string> NavigateToPage { get; }

    private readonly NavigationService _navigationService;

    private bool _isExpensesSelected = true;

    public BudgetViewModel()
    {
        Transactions = new ObservableCollection<Transaction>
        {
            new Transaction { Description = "Бургер Кинг", Category = "Фастфуд", Amount = "-299.97₽", Source = "ВТБ" },
            new Transaction { Description = "Николай Ж.", Category = "Переводы", Amount = "-299.97₽", Source = "СБЕР" }
        };
        
        _navigationService = new NavigationService();
        
        NavigateToPage = new RelayCommand<string>(_navigationService.NavigateToPageAsync);
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
}
using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WalletApp.Models;
using WalletApp.Services;

namespace WalletApp.ViewModels;

public partial class MainViewModel: ObservableObject, INotifyPropertyChanged
{
    public ObservableCollection<Item> Items { get; set; }
    public ObservableCollection<Transaction> Transactions { get; set; }
    public double ScreenWidth { get; private set; }
    public double ScreenWidthAdd25 { get; private set; }
    public List<Category> Categories { get; set; }
    
    public IRelayCommand<string> NavigateToPage { get; }
    
    private readonly NavigationService _navigationService;

    public MainViewModel()
    {
        ScreenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
        ScreenWidthAdd25 = ScreenWidth + 100;
        
        Items = new ObservableCollection<Item>
        {
            new Item { Title = "Общий счет", Description = "123 030\u20bd" },
            new Item { Title = "ВТБ", Description = "23 030\u20bd" },
            new Item { Title = "СБЕР", Description = "50 000\u20bd" },
            new Item { Title = "Т БАНК", Description = "50 000\u20bd" },
        };
        
        Transactions = new ObservableCollection<Transaction>
        {
            new Transaction { Description = "Бургер Кинг", Category = "Фастфуд", Amount = "-299.97₽", Source = "ВТБ" },
            new Transaction { Description = "Николай Ж.", Category = "Переводы", Amount = "-299.97₽", Source = "СБЕР" }
        };
        
        Categories = new List<Category>
        {
            new Category { Name = "Food", Value = 55, Color = Color.FromArgb("#494949") },
            new Category { Name = "Transport", Value = 15, Color = Color.FromArgb("#343434") },
            new Category { Name = "Entertainment", Value = 12, Color = Color.FromArgb("#787878") },
            new Category { Name = "Utilities", Value = 12, Color = Color.FromArgb("#000000") },
            new Category { Name = "Other", Value = 6, Color = Color.FromArgb("#E7E7E7E") },
        };

        _navigationService = new NavigationService();
        
        NavigateToPage = new RelayCommand<string>(_navigationService.NavigateToPageAsync);
    }
    
}
using System.Collections.ObjectModel;
using WalletApp.Models;

namespace WalletApp.ViewModels;

public class MainViewModel
{
    public ObservableCollection<Item> Items { get; set; }

    public MainViewModel()
    {
        Items = new ObservableCollection<Item>
        {
            new Item { Title = "Item 1", Description = "Description 1" },
            new Item { Title = "Item 2", Description = "Description 2" },
            new Item { Title = "Item 3", Description = "Description 3" },
            new Item { Title = "Item 4", Description = "Description 4" },
        };
    }
}
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WalletApp.Services;
using WalletApp.ViewModels.PopupModelView;
using WalletApp.Views.PopupViews;

namespace WalletApp.ViewModels;

public partial class SubscriptionViewModel: ObservableObject
{
    private readonly NavigationService _navigationService;
    static Page MainPage => Shell.Current;
    public IRelayCommand<string> NavigateToPage { get; }
    
    public SubscriptionViewModel()
    {
        _navigationService = new NavigationService();
        NavigateToPage = new RelayCommand<string>(_navigationService.NavigateToPageAsync);
    }
    
    [RelayCommand]
    private void OpenPopup(string title)
    {
        var v = new SimplePopupViewModel();

        switch (title)
        {
            case "1":
                v.Load("Подписка Яндекс Плюс, или погрузись в удивительный мир музыки и кино");
                break;
            case "2":
                v.Load("Насладитесь свежим воздухом, красивой плиткой, и зелеными насаждениями вместе с Воздух!");
                break;
            case "3":
                v.Load("Поддержите любимого автора и не дайте ему пойти на завод!");
                break;
            case "4":
                v.Load("Я не знаю что имел ввиду Захар, но вроде что-то развлекательное, пати-бас Fake Taxi?");
                break;
            case "5":
                v.Load("Будьте здоровыми, следите за своими показателями, в... Клинике Денис... Это вроде название сериала, не?");
                break;
        }
        
        var p = new SimplePopupView(v);
        MainPage.ShowPopup(p);
    }
}
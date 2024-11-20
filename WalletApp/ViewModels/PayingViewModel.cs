using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WalletApp.Services;

namespace WalletApp.ViewModels;

public partial class PayingViewModel: ObservableObject
{
    private readonly IAuthService _authService;
    private readonly IAPIService _apiService;
    public IRelayCommand<string> NavigateToPage { get; }

    private readonly NavigationService _navigationService;
    
    
    [RelayCommand]
    private async Task GoServices()
    {
        await Shell.Current.GoToAsync("//Services");
    }
    
    public IRelayCommand GoContacts { get; }
    
    private async void GoServicesAsync()
    {
        await Shell.Current.GoToAsync("//Contacts");
    }
    
    public PayingViewModel()
    {
        _authService = new AuthService();
        _apiService = new APIService(_authService);
        _navigationService = new NavigationService();

        GoContacts = new RelayCommand(GoServicesAsync);
        NavigateToPage = new RelayCommand<string>(_navigationService.NavigateToPageAsync);
        
    }
    
}
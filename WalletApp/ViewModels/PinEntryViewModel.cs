using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WalletApp.ViewModels;

public class PinEntryViewModel : ObservableObject
{
    private string _pin = string.Empty;
    public string Pin
    {
        get => _pin;
        set => SetProperty(ref _pin, value);
    }
    

    private string _errorMessage;
    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    private bool _hasError;

    public bool HasError
    {
        get => _hasError;
        set => SetProperty(ref _hasError, value);
    }

    public IRelayCommand<string> AddDigitCommand { get; }

    public PinEntryViewModel()
    {
        AddDigitCommand = new RelayCommand<string>(AddDigit);
    }
    
    private void AddDigit(string digit)
    {
        if (Pin.Length < 4)
        {
            Pin += digit;
        }

        if (Pin.Length == 4)
        {
            VerifyPinAsync();
        }
    }
    
    private async void VerifyPinAsync()
    {
        var savedPin = await SecureStorage.GetAsync("user_pin");
        
        if (Pin == savedPin)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            ErrorMessage = "Неверный PIN";
            HasError = true;
            Pin = string.Empty;
        }
    }
}
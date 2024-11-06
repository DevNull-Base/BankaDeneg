using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WalletApp.ViewModels;

public class PinEntryViewModel : ObservableObject
{
    private static Color disabled = new Color(142, 142, 142);
    private string _pin = string.Empty;
    public string Pin
    {
        get => _pin;
        set => SetProperty(ref _pin, value);
    }
    
    private ObservableCollection<Color> _pinIndicators = new ObservableCollection<Color>
    {
        disabled,
        disabled,
        disabled,
        disabled,
    };

    public ObservableCollection<Color> PinIndicators
    {
        get => _pinIndicators;
        set => SetProperty(ref _pinIndicators, value);
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
        
        var updatedIndicators = new ObservableCollection<Color>();
        for (int i = 0; i < 4; i++)
        {
            updatedIndicators.Add(Pin.Length > i ? Colors.White : disabled);
        }
        
        PinIndicators = updatedIndicators;

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
            
            var updatedIndicators = new ObservableCollection<Color>();
            for (int i = 0; i < 4; i++)
            {
                updatedIndicators.Add(disabled);
            }
        
            PinIndicators = updatedIndicators;
        }
    }
}
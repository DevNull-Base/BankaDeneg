using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WalletApp.ViewModels;

public class SetPinViewModel : ObservableObject
{
    private string _pin = string.Empty;

    public string Pin
    {
        get => _pin;
        set
        {
            SetProperty(ref _pin, value);
            OnPropertyChanged(nameof(IsSaveButtonEnabled));
        }
    }

    public bool IsSaveButtonEnabled => Pin.Length == 4;

    public IRelayCommand<string> AddDigitCommand { get; }
    public IRelayCommand RemoveLastDigitCommand { get; }
    public IAsyncRelayCommand SavePinCommand { get; }

    public SetPinViewModel()
    {
        AddDigitCommand = new RelayCommand<string>(AddDigit);
        RemoveLastDigitCommand = new RelayCommand(RemoveLastDigit);
        SavePinCommand = new AsyncRelayCommand(SavePinAsync);
    }

    private void AddDigit(string digit)
    {
        if (Pin.Length < 4)
        {
            Pin += digit;
        }
    }

    private void RemoveLastDigit()
    {
        if (!string.IsNullOrEmpty(Pin))
        {
            Pin = Pin.Remove(Pin.Length - 1);
        }
    }
    

    private async Task SavePinAsync()
    {
        if (Pin.Length == 4)
        {
            await SecureStorage.SetAsync("user_pin", Pin);
            await Shell.Current.GoToAsync("//MainPage");
        }
    }
    
}
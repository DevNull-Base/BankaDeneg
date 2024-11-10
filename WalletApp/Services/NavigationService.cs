namespace WalletApp.Services;

public class NavigationService
{
    public async void NavigateToPageAsync(string? page)
    {
        try
        {
            switch (page)
            {
                case "MainPage":
                    await Shell.Current.GoToAsync("//MainPage");
                    break;
                case "Payment":
                    await Shell.Current.GoToAsync("//Payment");
                    break;
                case "Budget":
                    await Shell.Current.GoToAsync("//Budget");
                    break;
                case "Card":
                    await Shell.Current.GoToAsync("//Accounts");
                    break;
                case "Setting":
                    await Shell.Current.GoToAsync("//Setting");
                    break;
            }
        }
        catch (Exception e)
        {
            return;
        }
    }
}
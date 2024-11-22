using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using WalletApp.Services;
using WalletApp.ViewModels;
using WalletApp.ViewModels.PopupModelView;
using WalletApp.Views;
using WalletApp.Views.PopupViews;

namespace WalletApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiCommunityToolkit()
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Inter-Regular.ttf", "InterRegular");
                fonts.AddFont("Inter-SemiBold.ttf", "InterSemiBold");
                fonts.AddFont("Inter-Medium.ttf", "InterMedium");
                fonts.AddFont("Inter-Bold.ttf", "InterBold");
                fonts.AddFont("Inter-ExtraLight.ttf", "InterExtraLight");
            })
            .Services
            .AddTransientPopup<SimplePopupView, SimplePopupViewModel>()
            .AddSingleton<IAuthService, AuthService>()
            .AddSingleton<IAPIService, APIService>()
            .AddSingleton<IDataService, DataService>()
            .AddSingleton<IBiometricService, BiometricService>()
            .AddSingleton<MainViewModel>()
            .AddSingleton<MainPage>()
            .AddSingleton<AccountViewModel>()
            .AddSingleton<AccountPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
        });
        
        return builder.Build();
    }
    

}
using Android.App;
using Android.Content.Res;
using Android.Runtime;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Handlers;

namespace WalletApp;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
        {
            handler.PlatformView.BackgroundTintList =
                ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
            if (view is Entry)
            {
                // Remove underline
                handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
            }
        });
        
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
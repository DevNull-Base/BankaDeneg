using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using WalletApp.ViewModels;

namespace WalletApp.Views;

public partial class SetPinPage : ContentPage
{
    public SetPinPage()
    {
        InitializeComponent();

        this.Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Colors.Transparent,
            StatusBarStyle = StatusBarStyle.LightContent
        });

        BindingContext = new SetPinViewModel();

        Btn.PropertyChanged += OnButtonPropertyChanged;
        Btn.BackgroundColor = Color.FromArgb("#4E4E4E");
    }

    private void OnButtonPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Button.IsEnabled))
        {
            AnimateButtonColor(Btn.IsEnabled ? Colors.White : Color.FromArgb("#4E4E4E"));
        }
    }

    // Метод для плавного изменения цвета кнопки
    private async void AnimateButtonColor(Color targetColor)
    {
        var currentColor = Btn.BackgroundColor;

        var animation = new Animation(v =>
        {
            Btn.BackgroundColor = Color.FromRgba(
                 currentColor.Red + (targetColor.Red - currentColor.Red) * v,
                 currentColor.Green + (targetColor.Green - currentColor.Green) * v,
                 currentColor.Blue + (targetColor.Blue - currentColor.Blue) * v,
                 currentColor.Alpha + (targetColor.Alpha - currentColor.Alpha) * v
            );
            
        }, 0, 1, Easing.Linear);
        
        animation.Commit(Btn, "ColorFade", 4, 125);
    }
}
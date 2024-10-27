using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.ViewModels;

namespace WalletApp.Views;

public partial class SetPinPage : ContentPage
{
    public SetPinPage()
    {
        InitializeComponent();
        BindingContext = new SetPinViewModel();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.ViewModels;

namespace WalletApp.Views;

public partial class PinEntryPage : ContentPage
{
    public PinEntryPage()
    {
        InitializeComponent();
        BindingContext = new PinEntryViewModel();
    }
}
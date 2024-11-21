using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;

namespace WalletApp.Views.PopupViews;

public partial class ExitPopupView : Popup
{
    public ExitPopupView()
    {
        InitializeComponent();
    }

    private void OnYesButtonClicked(object? sender, EventArgs e)
    {
        Close(true);
    }

    private void OnNoButtonClicked(object? sender, EventArgs e)
    {
        Close(false);
    }
}
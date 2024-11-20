using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using WalletApp.ViewModels.PopupModelView;

namespace WalletApp.Views.PopupViews;

public partial class SimplePopupView : Popup
{
    public SimplePopupView(SimplePopupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
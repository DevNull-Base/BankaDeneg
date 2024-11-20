using CommunityToolkit.Mvvm.ComponentModel;

namespace WalletApp.ViewModels.PopupModelView;

public partial class SimplePopupViewModel : ObservableObject
{
	[ObservableProperty]
	string title = "C# Binding Popup";

	[ObservableProperty]
	string message = "This message uses a ViewModel binding";

	internal void Load(string updatedMessage)
	{
		Message = updatedMessage;
	}
}
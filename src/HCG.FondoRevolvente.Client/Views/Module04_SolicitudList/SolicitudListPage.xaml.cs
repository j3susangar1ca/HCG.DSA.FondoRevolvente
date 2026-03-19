using Microsoft.Maui.Controls;

namespace HCG.FondoRevolvente.Client.Views.Module04_SolicitudList;

public partial class SolicitudListPage : ContentPage
{
	public SolicitudListPage()
	{
		InitializeComponent();
		BindingContext = new ViewModels.Module04_SolicitudList.SolicitudListViewModel();
	}
}

using Microsoft.UI.Xaml.Controls;

namespace HCG.FondoRevolvente.Client.Views.Module04_SolicitudList;

public sealed partial class SolicitudListPage : Page
{
    public SolicitudListPage()
    {
        InitializeComponent();
        DataContext = new ViewModels.Module04_SolicitudList.SolicitudListViewModel();
    }
}
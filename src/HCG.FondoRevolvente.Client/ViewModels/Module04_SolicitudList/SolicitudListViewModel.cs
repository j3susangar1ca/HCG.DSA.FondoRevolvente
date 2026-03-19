using CommunityToolkit.Mvvm.ComponentModel;

namespace HCG.FondoRevolvente.Client.ViewModels.Module04_SolicitudList;

public partial class SolicitudListViewModel : ObservableObject
{
    [ObservableProperty]
    private string _title = "Lista de Solicitudes";
}

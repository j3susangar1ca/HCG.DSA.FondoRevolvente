using HCG.FondoRevolvente.Domain.Enums;

namespace HCG.FondoRevolvente.Application.Solicitudes.Queries.GetSolicitudes;

public class SolicitudDto
{
    public int Id { get; set; }
    public string Folio { get; set; } = null!;
    public string Estado { get; set; } = null!;
    public decimal Monto { get; set; }
    public string Descripcion { get; set; } = null!;
    public string Solicitante { get; set; } = null!;
    public DateTime FechaCreacion { get; set; }
}

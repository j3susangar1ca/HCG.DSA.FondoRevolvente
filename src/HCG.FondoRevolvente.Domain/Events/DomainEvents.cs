using HCG.FondoRevolvente.Domain.Enums;
using HCG.FondoRevolvente.Domain.ValueObjects;

namespace HCG.FondoRevolvente.Domain.Events;

public record SolicitudCreadaEvent(int SolicitudId, FolioDSA Folio, string Solicitante);

public record EstadoCambiadoEvent(int SolicitudId, EstadoSolicitud NuevoEstado, string Usuario);

public record HitoRegistradoEvent(int SolicitudId, TipoHito Tipo, string Usuario);

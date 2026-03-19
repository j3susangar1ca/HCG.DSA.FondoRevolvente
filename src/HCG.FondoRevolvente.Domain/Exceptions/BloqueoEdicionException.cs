namespace HCG.FondoRevolvente.Domain.Exceptions;

/// <summary>RN-005 — Intento de edición sobre un expediente bloqueado por otro usuario.</summary>
public sealed class BloqueoEdicionException(string folioSolicitud, string usuarioEditor, DateTime bloqueadoDesde)
    : DomainException(
        "BLOQUEO_EDICION_RN005",
        $"El expediente '{folioSolicitud}' está siendo editado por {usuarioEditor} " +
        $"desde las {bloqueadoDesde:HH:mm}. (RN-005)")
{
    public string FolioSolicitud { get; } = folioSolicitud;
    public string UsuarioEditor { get; } = usuarioEditor;
    public DateTime BloqueadoDesde { get; } = bloqueadoDesde;
}

using HCG.FondoRevolvente.Domain.Interfaces;
using System.Collections.Concurrent;
using HCG.FondoRevolvente.Domain.Constants;

namespace HCG.FondoRevolvente.Domain.Services;

/// <summary>
/// Gestión de bloqueos en memoria (Singleton/In-Process).
/// Para entornos distribuidos se recomienda usar Distributed Cache (Redis).
/// §RN-005.
/// </summary>
public class BloqueoEdicionService : IBloqueoEdicionService
{
    private static readonly ConcurrentDictionary<string, (string Usuario, DateTime Expiracion)> _bloqueos = new();

    public bool TryAcquireLock(string folio, string usuario, out string? poseedorActual)
    {
        LimpiarBloqueosExpirados();
        poseedorActual = null;

        if (_bloqueos.TryGetValue(folio, out var bloqueo))
        {
            if (bloqueo.Usuario == usuario)
            {
                RenovarLock(folio, usuario);
                return true;
            }
            poseedorActual = bloqueo.Usuario;
            return false;
        }

        var expiracion = DateTime.UtcNow.AddMinutes(LimitesNegocio.MinutosDuracionBloqueo);
        return _bloqueos.TryAdd(folio, (usuario, expiracion));
    }

    public void ReleaseLock(string folio, string usuario)
    {
        if (_bloqueos.TryGetValue(folio, out var bloqueo) && bloqueo.Usuario == usuario)
        {
            _bloqueos.TryRemove(folio, out _);
        }
    }

    public bool IsLockedByOther(string folio, string usuario, out string? poseedorActual)
    {
        LimpiarBloqueosExpirados();
        poseedorActual = null;

        if (_bloqueos.TryGetValue(folio, out var bloqueo) && bloqueo.Usuario != usuario)
        {
            poseedorActual = bloqueo.Usuario;
            return true;
        }

        return false;
    }

    private void RenovarLock(string folio, string usuario)
    {
        var expiracion = DateTime.UtcNow.AddMinutes(LimitesNegocio.MinutosDuracionBloqueo);
        _bloqueos[folio] = (usuario, expiracion);
    }

    private void LimpiarBloqueosExpirados()
    {
        var ahora = DateTime.UtcNow;
        foreach (var item in _bloqueos)
        {
            if (item.Value.Expiracion < ahora)
            {
                _bloqueos.TryRemove(item.Key, out _);
            }
        }
    }
}

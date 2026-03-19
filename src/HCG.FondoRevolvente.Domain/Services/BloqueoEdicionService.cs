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
    private static readonly ConcurrentDictionary<int, BloqueoEntry> _bloqueos = new();

    private sealed record BloqueoEntry(string Usuario, string NombreCompleto, DateTime Expiracion);

    public Task<bool> AdquirirBloqueoAsync(
        int solicitudId,
        string usuario,
        string nombreCompleto,
        CancellationToken cancellationToken = default)
    {
        LimpiarBloqueosExpirados();

        if (_bloqueos.TryGetValue(solicitudId, out var bloqueo))
        {
            if (bloqueo.Usuario == usuario)
            {
                // Renovar bloqueo existente
                var expiracion = DateTime.UtcNow.AddMinutes(LimitesNegocio.MinutosDuracionBloqueo);
                _bloqueos[solicitudId] = new BloqueoEntry(usuario, nombreCompleto, expiracion);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        var nuevaExpiracion = DateTime.UtcNow.AddMinutes(LimitesNegocio.MinutosDuracionBloqueo);
        var result = _bloqueos.TryAdd(solicitudId, new BloqueoEntry(usuario, nombreCompleto, nuevaExpiracion));
        return Task.FromResult(result);
    }

    public Task LiberarBloqueoAsync(int solicitudId, string usuario, CancellationToken cancellationToken = default)
    {
        if (_bloqueos.TryGetValue(solicitudId, out var bloqueo) && bloqueo.Usuario == usuario)
        {
            _bloqueos.TryRemove(solicitudId, out _);
        }
        return Task.CompletedTask;
    }

    public Task<bool> RenovarBloqueoAsync(int solicitudId, string usuario, CancellationToken cancellationToken = default)
    {
        if (_bloqueos.TryGetValue(solicitudId, out var bloqueo) && bloqueo.Usuario == usuario)
        {
            var expiracion = DateTime.UtcNow.AddMinutes(LimitesNegocio.MinutosDuracionBloqueo);
            _bloqueos[solicitudId] = bloqueo with { Expiracion = expiracion };
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<bool> EstaBloqueadaAsync(int solicitudId, CancellationToken cancellationToken = default)
    {
        LimpiarBloqueosExpirados();
        return Task.FromResult(_bloqueos.ContainsKey(solicitudId));
    }

    public Task<InfoBloqueo?> ObtenerInfoBloqueoAsync(int solicitudId, CancellationToken cancellationToken = default)
    {
        LimpiarBloqueosExpirados();

        if (_bloqueos.TryGetValue(solicitudId, out var bloqueo))
        {
            var ahora = DateTime.UtcNow;
            var info = new InfoBloqueo
            {
                Usuario = bloqueo.Usuario,
                NombreCompleto = bloqueo.NombreCompleto,
                FechaAdquisicion = bloqueo.Expiracion.AddMinutes(-LimitesNegocio.MinutosDuracionBloqueo),
                TiempoRestante = bloqueo.Expiracion - ahora,
                EstaExpirado = bloqueo.Expiracion < ahora
            };
            return Task.FromResult<InfoBloqueo?>(info);
        }

        return Task.FromResult<InfoBloqueo?>(null);
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

using HCG.FondoRevolvente.Domain.Constants;
using HCG.FondoRevolvente.Domain.Interfaces;

namespace HCG.FondoRevolvente.Domain.Services;

/// <summary>
/// Servicio de dominio para gestionar el bloqueo de edición de solicitudes.
/// Implementa RN-005: Bloqueo optimista de edición para prevenir conflictos de concurrencia.
/// NOTA: Esta es una implementación de dominio en memoria. La persistencia persistente se manejaría en infraestructura.
/// </summary>
public class BloqueoEdicionService : IBloqueoEdicionService
{
    // Almacenamiento en memoria para bloqueos (en producción usar Redis o base de datos)
    private readonly Dictionary<int, InfoBloqueo> _bloqueos = new();

    /// <inheritdoc />
    public Task<bool> AdquirirBloqueoAsync(
        int solicitudId,
        string usuario,
        string nombreCompleto,
        CancellationToken cancellationToken = default)
    {
        // Verificar si ya existe un bloqueo
        if (_bloqueos.TryGetValue(solicitudId, out var bloqueoExistente))
        {
            // Si el bloqueo está expirado, eliminarlo
            if (ConfiguracionBloqueo.EstaExpirado(bloqueoExistente.FechaAdquisicion, DateTime.UtcNow))
            {
                _bloqueos.Remove(solicitudId);
            }
            else if (bloqueoExistente.Usuario != usuario)
            {
                // El bloqueo está activo y pertenece a otro usuario
                return Task.FromResult(false);
            }
            // Si el mismo usuario tiene el bloqueo, se renueva al final
        }

        // Crear/Renovar bloqueo
        var nuevoBloqueo = new InfoBloqueo
        {
            Usuario = usuario,
            NombreCompleto = nombreCompleto,
            FechaAdquisicion = DateTime.UtcNow,
            TiempoRestante = ConfiguracionBloqueo.DuracionBloqueo,
            EstaExpirado = false
        };

        _bloqueos[solicitudId] = nuevoBloqueo;
        return Task.FromResult(true);
    }

    /// <inheritdoc />
    public Task LiberarBloqueoAsync(int solicitudId, string usuario, CancellationToken cancellationToken = default)
    {
        if (_bloqueos.TryGetValue(solicitudId, out var bloqueo) && bloqueo.Usuario == usuario)
        {
            _bloqueos.Remove(solicitudId);
        }
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<bool> RenovarBloqueoAsync(int solicitudId, string usuario, CancellationToken cancellationToken = default)
    {
        if (!_bloqueos.TryGetValue(solicitudId, out var bloqueo))
            return Task.FromResult(false);

        if (bloqueo.Usuario != usuario)
            return Task.FromResult(false);

        if (ConfiguracionBloqueo.EstaExpirado(bloqueo.FechaAdquisicion, DateTime.UtcNow))
        {
            _bloqueos.Remove(solicitudId);
            return Task.FromResult(false);
        }

        // Renovar el bloqueo
        _bloqueos[solicitudId] = new InfoBloqueo
        {
            Usuario = bloqueo.Usuario,
            NombreCompleto = bloqueo.NombreCompleto,
            FechaAdquisicion = DateTime.UtcNow,
            TiempoRestante = ConfiguracionBloqueo.DuracionBloqueo,
            EstaExpirado = false
        };

        return Task.FromResult(true);
    }

    /// <inheritdoc />
    public Task<bool> EstaBloqueadaAsync(int solicitudId, CancellationToken cancellationToken = default)
    {
        if (!_bloqueos.TryGetValue(solicitudId, out var bloqueo))
            return Task.FromResult(false);

        if (ConfiguracionBloqueo.EstaExpirado(bloqueo.FechaAdquisicion, DateTime.UtcNow))
        {
            _bloqueos.Remove(solicitudId);
            return Task.FromResult(false);
        }

        return Task.FromResult(true);
    }

    /// <inheritdoc />
    public Task<InfoBloqueo?> ObtenerInfoBloqueoAsync(int solicitudId, CancellationToken cancellationToken = default)
    {
        if (!_bloqueos.TryGetValue(solicitudId, out var bloqueo))
            return Task.FromResult<InfoBloqueo?>(null);

        if (ConfiguracionBloqueo.EstaExpirado(bloqueo.FechaAdquisicion, DateTime.UtcNow))
        {
            _bloqueos.Remove(solicitudId);
            return Task.FromResult<InfoBloqueo?>(null);
        }

        var tiempoRestante = ConfiguracionBloqueo.TiempoRestante(bloqueo.FechaAdquisicion, DateTime.UtcNow);

        return Task.FromResult<InfoBloqueo?>(new InfoBloqueo
        {
            Usuario = bloqueo.Usuario,
            NombreCompleto = bloqueo.NombreCompleto,
            FechaAdquisicion = bloqueo.FechaAdquisicion,
            TiempoRestante = tiempoRestante,
            EstaExpirado = false
        });
    }

    /// <summary>
    /// Limpia todos los bloqueos expirados (mantenimiento periódico).
    /// </summary>
    public void LimpiarBloqueosExpirados()
    {
        var ahora = DateTime.UtcNow;
        var expirados = _bloqueos
            .Where(kvp => ConfiguracionBloqueo.EstaExpirado(kvp.Value.FechaAdquisicion, ahora))
            .Select(kvp => kvp.Key)
            .ToList();

        foreach (var key in expirados)
        {
            _bloqueos.Remove(key);
        }
    }
}

namespace HCG.FondoRevolvente.Domain.Interfaces;

/// <summary>
/// Interfaz genérica para repositorios del dominio.
/// </summary>
/// <typeparam name="T">Tipo de la entidad.</typeparam>
/// <typeparam name="TId">Tipo del identificador.</typeparam>
public interface IRepository<T, TId> where T : class
{
    /// <summary>
    /// Obtiene una entidad por su identificador.
    /// </summary>
    Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene todas las entidades.
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Agrega una nueva entidad.
    /// </summary>
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Actualiza una entidad existente.
    /// </summary>
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Elimina una entidad.
    /// </summary>
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica si existe una entidad con el identificador especificado.
    /// </summary>
    Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default);
}

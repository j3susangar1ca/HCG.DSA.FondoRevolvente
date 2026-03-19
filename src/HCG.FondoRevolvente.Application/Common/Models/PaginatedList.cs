namespace HCG.FondoRevolvente.Application.Common.Models;

/// <summary>
/// Representa una lista paginada de elementos con metadatos de paginación.
/// </summary>
/// <typeparam name="T">Tipo de los elementos.</typeparam>
public class PaginatedList<T> : IReadOnlyList<T>
{
    private readonly IReadOnlyList<T> _items;

    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public int TotalPages { get; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
    public int FirstItemIndex => (PageNumber - 1) * PageSize + 1;
    public int LastItemIndex => Math.Min(PageNumber * PageSize, TotalCount);
    public bool IsEmpty => _items.Count == 0;
    public int Count => _items.Count;
    public T this[int index] => _items[index];

    /// <summary>
    /// Elementos de la página actual.
    /// Agregado para compatibilidad con la estructura JSON esperada.
    /// </summary>
    public IReadOnlyList<T> Items => _items;

    public PaginatedList(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
        _items = items.ToList().AsReadOnly();
        TotalCount = totalCount;
        PageNumber = Math.Max(1, pageNumber);
        PageSize = Math.Max(1, pageSize);
        TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
    }

    public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _items.GetEnumerator();

    /// <summary>
    /// Crea una lista paginada desde un IQueryable de forma eficiente.
    /// </summary>
    public static async Task<PaginatedList<T>> CreateAsync(
        IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await Task.Run(() => source.Count(), cancellationToken);
        var items = await Task.Run(
            () => source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(),
            cancellationToken);
        return new PaginatedList<T>(items, totalCount, pageNumber, pageSize);
    }

    /// <summary>
    /// Crea una lista paginada vacía.
    /// </summary>
    public static PaginatedList<T> Empty() => new(Array.Empty<T>(), 0, 1, 10);

    /// <summary>
    /// Obtiene los metadatos de paginación.
    /// </summary>
    public PaginationMetadata GetMetadata() => new()
    {
        PageNumber = PageNumber,
        PageSize = PageSize,
        TotalCount = TotalCount,
        TotalPages = TotalPages,
        HasPreviousPage = HasPreviousPage,
        HasNextPage = HasNextPage,
        FirstItemIndex = FirstItemIndex,
        LastItemIndex = LastItemIndex
    };

    /// <summary>
    /// Transforma los elementos a otro tipo.
    /// </summary>
    public PaginatedList<TDestination> Map<TDestination>(Func<T, TDestination> selector)
        => new(_items.Select(selector), TotalCount, PageNumber, PageSize);
}

/// <summary>
/// Metadatos de paginación para respuestas API.
/// </summary>
public class PaginationMetadata
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
    public bool HasPreviousPage { get; init; }
    public bool HasNextPage { get; init; }
    public int FirstItemIndex { get; init; }
    public int LastItemIndex { get; init; }
}

/// <summary>
/// Parámetros de paginación para consultas.
/// </summary>
public class PaginationParameters
{
    private int _pageNumber = 1;
    private int _pageSize = 10;
    private const int MaxPageSize = 100;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = Math.Max(1, value);
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = Math.Min(Math.Max(1, value), MaxPageSize);
    }

    public string? SortBy { get; set; }
    public bool SortDescending { get; set; }
    public string? SearchTerm { get; set; }
}

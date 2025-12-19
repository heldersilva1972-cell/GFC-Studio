using System;
using System.Collections.Generic;

namespace GFC.Core.Models;

/// <summary>
/// Standard paginated result wrapper used by read-only queries.
/// </summary>
/// <typeparam name="T">The item type returned in the page.</typeparam>
public class PagedResult<T>
{
    public PagedResult(IReadOnlyList<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items ?? throw new ArgumentNullException(nameof(items));
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public IReadOnlyList<T> Items { get; }

    public int TotalCount { get; }

    public int PageNumber { get; }

    public int PageSize { get; }
}

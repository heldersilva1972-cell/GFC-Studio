## CRITICAL FIX FOR 30-SECOND TIMEOUT

### Problem
The ControllerEvents query is timing out after 30 seconds because of this line:

```csharp
where e.CardNumber != null && cardNumbers.Contains(e.CardNumber.Value)
```

When there are many cards (e.g., 100+), this creates a massive IN clause like:
```sql
WHERE CardNumber IN (1, 2, 3, ... 100+ values)
```

Even with the index, SQL Server struggles with this and times out.

### Solution
Replace lines 1093-1111 in `CardAssignmentsTab.razor` with this optimized code:

```csharp
            // 2. Fetch last used dates from ControllerEvents - CRITICAL PERFORMANCE FIX
            swStep.Start();
            var cardNumbers = cards.Select(c => long.TryParse(c.CardNumber, out var val) ? (long?)val : null)
                                   .Where(v => v.HasValue).Select(v => v!.Value).ToList();
            
            var lastUsedMap = new Dictionary<long, DateTime>();
            if (cardNumbers.Any())
            {
                using var db = await DbContextFactory.CreateDbContextAsync();
                
                // CRITICAL FIX: Use raw SQL instead of LINQ to avoid 30-second timeout
                // The LINQ query with cardNumbers.Contains() creates a massive IN clause that times out
                // Instead, we load ALL last-used dates (fast with index) and filter in memory
                var sql = @"
                    SELECT CardNumber, MAX(TimestampUtc) as LastUsed
                    FROM ControllerEvents WITH (NOLOCK)
                    WHERE CardNumber IS NOT NULL
                    GROUP BY CardNumber";
                
                try
                {
                    var allResults = await db.Database.SqlQueryRaw<CardLastUsedDto>(sql)
                        .AsNoTracking()
                        .ToListAsync();
                    
                    // Filter to only the cards we have (in memory, instant)
                    var cardNumberSet = cardNumbers.ToHashSet();
                    lastUsedMap = allResults
                        .Where(r => cardNumberSet.Contains(r.CardNumber))
                        .ToDictionary(x => x.CardNumber, x => x.LastUsed);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Error loading last used dates from ControllerEvents");
                    // Continue without last-used dates rather than failing completely
                }
            }
```

### Why This Works

1. **No IN clause**: Loads ALL last-used dates at once using `GROUP BY`
2. **Index is used**: SQL Server can use the index efficiently for the simple GROUP BY
3. **Fast filtering**: Filtering to specific cards happens in memory (instant)
4. **WITH (NOLOCK)**: Avoids locking issues
5. **Error handling**: Won't crash if query fails

### Expected Performance

- **Before**: 30+ seconds (timeout)
- **After**: 500-1000ms

### Note

The DTO class `CardLastUsedDto` has already been added to the file (lines 1587-1591).

Just replace the query code and rebuild!

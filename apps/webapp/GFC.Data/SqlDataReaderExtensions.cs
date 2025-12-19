using Microsoft.Data.SqlClient;

namespace GFC.Data;

/// <summary>
/// Helper extensions for working with <see cref="SqlDataReader"/> so that
/// SELECT statements and mapping logic stay in sync.
/// </summary>
internal static class SqlDataReaderExtensions
{
    /// <summary>
    /// Ensures that the supplied <paramref name="columnNames"/> all exist in the
    /// current <see cref="SqlDataReader"/> result set. Throws an
    /// <see cref="InvalidOperationException"/> with a descriptive message when a
    /// column is missing so developers can correct the offending SELECT quickly.
    /// </summary>
    /// <param name="reader">The active data reader.</param>
    /// <param name="context">Repository method (e.g. MemberRepository.GetAllMembers).</param>
    /// <param name="columnNames">Columns that must be present.</param>
    public static void EnsureColumns(
        this SqlDataReader reader,
        string context,
        params string[] columnNames)
    {
        foreach (var columnName in columnNames)
        {
            try
            {
                reader.GetOrdinal(columnName);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new InvalidOperationException(
                    $"Expected column '{columnName}' not found in result set for {context}. " +
                    "Update the SELECT statement so the mapping and SQL stay aligned.",
                    ex);
            }
        }
    }
}


// [NEW]
namespace GFC.Core.Models.Diagnostics
{
    /// <summary>
    /// Represents the result of a diagnostic action.
    /// </summary>
    public class DiagnosticActionResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the action was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets a message describing the result of the action.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the time taken to execute the action, in milliseconds.
        /// </summary>
        public long ResponseTimeMs { get; set; }
    }
}

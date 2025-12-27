// [NEW]
using GFC.Core.Models;
using System.Threading.Tasks;

namespace GFC.Core.Interfaces
{
    /// <summary>
    /// Service for managing dynamic form definitions.
    /// </summary>
    public interface IFormBuilderService
    {
        /// <summary>
        /// Gets a form definition by its unique name.
        /// </summary>
        /// <param name="name">The unique name of the form.</param>
        /// <returns>The DynamicForm if found; otherwise, null.</returns>
        Task<DynamicForm> GetFormByNameAsync(string name);

        /// <summary>
        /// Creates or updates a form definition.
        /// </summary>
        /// <param name="form">The form definition to save.</param>
        Task SaveFormAsync(DynamicForm form);
    }
}

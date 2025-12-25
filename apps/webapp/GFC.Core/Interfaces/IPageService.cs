// [NEW]
using GFC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.Core.Interfaces
{
    public interface IPageService
    {
        /// <summary>
        /// Gets a list of all pages available in the Studio.
        /// </summary>
        /// <returns>A list of StudioPage objects.</returns>
        Task<List<StudioPage>> GetAllPagesAsync();

        /// <summary>
        /// Retrieves a specific page by its ID.
        /// </summary>
        /// <param name="pageId">The ID of the page to retrieve.</param>
        /// <returns>The StudioPage object, or null if not found.</returns>
        Task<StudioPage> GetPageAsync(int pageId);

        /// <summary>
        /// Creates a new page.
        /// </summary>
        /// <param name="title">The title of the new page.</param>
        /// <param name="cloneFromPageId">Optional. If provided, the new page will be a clone of the page with this ID.</param>
        /// <returns>The newly created StudioPage object.</returns>
        Task<StudioPage> CreatePageAsync(string title, int? cloneFromPageId = null);

        /// <summary>
        /// Updates an existing page's properties.
        /// </summary>
        /// <param name="page">The page object with updated values.</param>
        /// <returns>Task representing the async operation.</returns>
        Task UpdatePageAsync(StudioPage page);

        /// <summary>
        /// Deletes a page and all its associated content.
        /// </summary>
        /// <param name="pageId">The ID of the page to delete.</param>
        /// <returns>Task representing the async operation.</returns>
        Task DeletePageAsync(int pageId);
    }
}

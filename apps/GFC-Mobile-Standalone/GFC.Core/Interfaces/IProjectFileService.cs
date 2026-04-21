using GFC.Core.Models;
using System.Threading.Tasks;

namespace GFC.Core.Interfaces
{
    public interface IProjectFileService
    {
        /// <summary>
        /// Initializes the required folder structure in the given project root.
        /// </summary>
        Task InitializeProjectStructureAsync(string projectRoot);

        /// <summary>
        /// Saves a StudioPage and its sections as a JSON data file for the Studio.
        /// </summary>
        Task SavePageDataAsync(string projectRoot, StudioPage page, List<StudioSection> sections);

        /// <summary>
        /// Generates a physical file (e.g. .html or .tsx) for the page.
        /// </summary>
        Task GeneratePageFileAsync(string projectRoot, StudioPage page, List<StudioSection> sections);
        
        /// <summary>
        /// Synchronizes assets to the project's public folder.
        /// </summary>
        Task SyncAssetsAsync(string projectRoot, string sourceUploadsFolder);
    }
}

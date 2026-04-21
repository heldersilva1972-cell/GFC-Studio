// [NEW]
using GFC.Core.Models;
using System.Threading.Tasks;

namespace GFC.Core.Interfaces
{
    public interface IAnimationService
    {
        Task<AnimationDefinition> GetAnimationByIdAsync(string id);
    }
}

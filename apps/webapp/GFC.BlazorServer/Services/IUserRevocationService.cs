using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface IUserRevocationService
    {
        Task RevokeUserAccessAsync(int userId, string reason, int performedByUserId);
    }
}

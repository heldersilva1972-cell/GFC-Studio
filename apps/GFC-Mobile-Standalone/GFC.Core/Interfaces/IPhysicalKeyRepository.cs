using GFC.Core.Models;

namespace GFC.Core.Interfaces
{
    /// <summary>
    /// Repository for physical door key tracking operations.
    /// </summary>
    public interface IPhysicalKeyRepository
    {
        List<PhysicalKey> GetAllKeys();
        List<PhysicalKey> GetActiveKeys(); // Keys that haven't been returned
        List<PhysicalKey> GetKeysForMember(int memberId);
        PhysicalKey? GetKeyById(int keyId);
        int IssueKey(PhysicalKey key);
        void ReturnKey(int keyId, DateTime returnedDate, string? returnedBy);
        void UpdateKey(PhysicalKey key);
        void DeleteKey(int keyId);
    }
}


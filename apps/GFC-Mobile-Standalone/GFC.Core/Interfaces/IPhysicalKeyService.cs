using GFC.Core.Models;

namespace GFC.Core.Interfaces
{
    /// <summary>
    /// Service for physical door key management operations.
    /// </summary>
    public interface IPhysicalKeyService
    {
        List<PhysicalKey> GetAllKeys();
        List<PhysicalKey> GetActiveKeys();
        List<PhysicalKey> GetKeysForMember(int memberId);
        PhysicalKey? GetKeyById(int keyId);
        int IssueKey(int memberId, DateTime issuedDate, string? issuedBy, string? notes);
        void ReturnKey(int keyId, DateTime returnedDate, string? returnedBy);
        void UpdateKey(PhysicalKey key);
        void DeleteKey(int keyId);
        
        // Alert methods
        List<PhysicalKey> GetKeysThatShouldBeReturned(); // Keys assigned to non-board members
        bool MemberHasActiveKey(int memberId);
        bool IsMemberCurrentBoardMember(int memberId);
    }
}


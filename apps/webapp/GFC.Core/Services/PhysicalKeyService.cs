using GFC.Core.Interfaces;
using GFC.Core.Models;

namespace GFC.Core.Services
{
    /// <summary>
    /// Service for physical door key management operations.
    /// </summary>
    public class PhysicalKeyService : IPhysicalKeyService
    {
        private readonly IPhysicalKeyRepository _repository;
        private readonly IBoardRepository _boardRepository;
        private readonly IAuditLogger _auditLogger;

        public PhysicalKeyService(
            IPhysicalKeyRepository repository,
            IBoardRepository boardRepository,
            IAuditLogger auditLogger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _boardRepository = boardRepository ?? throw new ArgumentNullException(nameof(boardRepository));
            _auditLogger = auditLogger ?? throw new ArgumentNullException(nameof(auditLogger));
        }

        public List<PhysicalKey> GetAllKeys()
        {
            return _repository.GetAllKeys();
        }

        public List<PhysicalKey> GetActiveKeys()
        {
            return _repository.GetActiveKeys();
        }

        public List<PhysicalKey> GetKeysForMember(int memberId)
        {
            return _repository.GetKeysForMember(memberId);
        }

        public PhysicalKey? GetKeyById(int keyId)
        {
            return _repository.GetKeyById(keyId);
        }

        public int IssueKey(int memberId, DateTime issuedDate, string? issuedBy, string? notes)
        {
            var key = new PhysicalKey
            {
                MemberID = memberId,
                IssuedDate = issuedDate,
                IssuedBy = issuedBy,
                Notes = notes
            };

            var keyId = _repository.IssueKey(key);
            
            var issuedKey = _repository.GetKeyById(keyId);
            var memberName = issuedKey?.MemberName ?? $"Member #{memberId}";

            var details = $"[{memberName}] Issued physical key {keyId} on {issuedDate:d} by {issuedBy ?? "unknown"}; notes: {notes ?? "none"}";
            _auditLogger.Log(
                AuditLogActions.PhysicalKeyAssigned,
                null,
                null,
                details,
                targetMemberId: memberId);

            return keyId;
        }

        public void ReturnKey(int keyId, DateTime returnedDate, string? returnedBy)
        {
            _repository.ReturnKey(keyId, returnedDate, returnedBy);

            var key = _repository.GetKeyById(keyId);
            var memberId = key?.MemberID ?? 0;
            var memberName = key?.MemberName ?? "Unknown Member";
            var details = $"[{memberName}] Returned key {keyId} on {returnedDate:d} by {returnedBy ?? "unknown"}";
            _auditLogger.Log(
                AuditLogActions.PhysicalKeyReturned,
                null,
                null,
                details,
                targetMemberId: memberId > 0 ? memberId : null);
        }

        public void UpdateKey(PhysicalKey key)
        {
            _repository.UpdateKey(key);
        }

        public void DeleteKey(int keyId)
        {
            _repository.DeleteKey(keyId);
        }

        public List<PhysicalKey> GetKeysThatShouldBeReturned()
        {
            var activeKeys = _repository.GetActiveKeys();
            var keysToReturn = new List<PhysicalKey>();

            foreach (var key in activeKeys)
            {
                // Check if member is currently a board member
                if (!IsMemberCurrentBoardMember(key.MemberID))
                {
                    keysToReturn.Add(key);
                }
            }

            return keysToReturn;
        }

        public bool MemberHasActiveKey(int memberId)
        {
            var keys = _repository.GetKeysForMember(memberId);
            return keys.Any(k => !k.IsReturned);
        }

        public bool IsMemberCurrentBoardMember(int memberId)
        {
            var mostRecentYear = _boardRepository.GetMostRecentTermYear();
            if (!mostRecentYear.HasValue)
                return false;

            return _boardRepository.IsBoardMemberForYear(memberId, mostRecentYear.Value);
        }
    }
}


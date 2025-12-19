using GFC.Core.Models;

namespace GFC.Core.Interfaces;

/// <summary>
/// Provides aggregated member/key-card data for a specific dues year.
/// </summary>
public interface IKeyCardDashboardRepository
{
    List<KeyCardMemberRow> GetMembersForYear(int year);
}


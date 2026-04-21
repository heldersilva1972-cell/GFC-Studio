using GFC.Core.Models;

namespace GFC.Core.Interfaces;

public interface IKeyCardRepository
{
    KeyCard? GetById(int keyCardId);
    KeyCard? GetByCardNumber(string cardNumber);
    List<KeyCard> GetAll();
    KeyCard Create(string cardNumber, int memberId, string? notes, string cardType = "Card");
    KeyCard? ReplaceCard(int memberId, string newCardNumber, string? notes, string cardType = "Card");
    void Update(KeyCard card);
    void Delete(int keyCardId);
    KeyCard? GetActiveMemberCard(int memberId);
}


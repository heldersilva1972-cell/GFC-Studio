using GFC.Core.Models;

namespace GFC.Core.Interfaces;

public interface IKeyCardRepository
{
    KeyCard? GetById(int keyCardId);
    KeyCard? GetByCardNumber(string cardNumber);
    List<KeyCard> GetAll();
    KeyCard Create(string cardNumber, int memberId, string? notes);
    void Update(KeyCard card);
}


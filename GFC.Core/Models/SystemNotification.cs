// [NEW]
using System;

namespace GFC.Core.Models;

public class SystemNotification
{
    public int Id { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedDate { get; set; }
}

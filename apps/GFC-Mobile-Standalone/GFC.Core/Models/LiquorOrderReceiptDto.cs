using System.Collections.Generic;

namespace GFC.Core.Models
{
    public class LiquorOrderReceiptDto
    {
        public int OrderId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public decimal TotalDue { get; set; }
        public List<int> BackorderedOrderItemIds { get; set; } = new List<int>();
        public int UserId { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace GFC.BlazorServer.Components.Pages.Admin.Pos
{
    public class ProductItem
    {
        public Guid CartItemId { get; set; } = Guid.NewGuid();
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 1;
        public string Category { get; set; } = "MISC";
        public bool IsTokenApplied { get; set; }
        public int DisplayOrder { get; set; }
        public List<ProductItem> Modifiers { get; set; } = new();
        public Guid? AssociatedCartItemId { get; set; }
        public int? AppliedTokenId { get; set; }
        public int ZReportGroup { get; set; }
    }
}

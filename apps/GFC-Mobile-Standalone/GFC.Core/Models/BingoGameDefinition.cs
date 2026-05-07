using System;

namespace GFC.Core.Models
{
    public class BingoGameDefinition : BaseEntity
    {
        public int Id { get; set; }
        public int SheetDefinitionId { get; set; }
        public string GameName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; }
        public decimal DefaultPayout { get; set; }
        public bool IsVariablePayout { get; set; }
        public virtual BingoSheetDefinition Sheet { get; set; } = null!;
    }
}

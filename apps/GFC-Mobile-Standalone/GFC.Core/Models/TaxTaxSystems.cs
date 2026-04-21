namespace GFC.Core.Models;

public class TaxBracket
{
    public int Id { get; set; }
    public int TaxYear { get; set; }
    public string FilingStatus { get; set; } = "Single"; // Single, Married, HeadOfHousehold
    public decimal LowerBound { get; set; }
    public decimal? UpperBound { get; set; }
    public decimal BaseTax { get; set; }
    public decimal Rate { get; set; } // e.g. 0.22
}

public class TaxStandardDeduction
{
    public int Id { get; set; }
    public int TaxYear { get; set; }
    public string FilingStatus { get; set; } = "Single";
    public decimal Amount { get; set; }
}

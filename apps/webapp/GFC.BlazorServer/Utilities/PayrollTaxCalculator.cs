using GFC.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace GFC.BlazorServer.Utilities
{
    public static class PayrollTaxCalculator
    {
        // FICA Rates (2024 Locked Standard)
        private const decimal FicaSocialSecurityRate = 0.062m;
        private const decimal FicaMedicareRate = 0.0145m;

        public class TaxBreakdown
        {
            public decimal FederalTax { get; set; }
            public decimal SocialSecurity { get; set; }
            public decimal Medicare { get; set; }
            public decimal TotalEmployeeWithholding => FederalTax + SocialSecurity + Medicare;

            public decimal EmployerSocialSecurity { get; set; }
            public decimal EmployerMedicare { get; set; }
            public decimal TotalEmployerLiability => EmployerSocialSecurity + EmployerMedicare;
        }

        public static TaxBreakdown CalculateFederalTaxes(
            decimal gross, 
            AppUser profile, 
            List<TaxBracket> yearBrackets, 
            TaxStandardDeduction yearDeduction, 
            string payFrequency = "Monthly")
        {
            var result = new TaxBreakdown();
            decimal periodsInYear = payFrequency == "Monthly" ? 12m : 52m;

            // 1. Calculate FICA (Social Security & Medicare)
            result.SocialSecurity = gross * FicaSocialSecurityRate;
            result.Medicare = gross * FicaMedicareRate;
            result.EmployerSocialSecurity = gross * FicaSocialSecurityRate;
            result.EmployerMedicare = gross * FicaMedicareRate;

            // 2. Calculate Taxable Income
            decimal annualGross = (gross * periodsInYear) + profile.OtherIncomeAmount;
            
            // Use dynamic deduction from DB
            decimal standardDeduction = yearDeduction?.Amount ?? 14600m; // Fallback to 2024 single

            decimal taxableIncome = annualGross - standardDeduction - profile.DeductionsAmount;
            if (taxableIncome <= 0) 
            {
                result.FederalTax = profile.ExtraWithholdingAmount; 
                return result;
            }

            // 3. Apply Dynamic Brackets from DB
            decimal annualTax = 0;
            var relevantBrackets = yearBrackets
                .Where(b => b.FilingStatus == profile.FilingStatus)
                .OrderBy(b => b.LowerBound)
                .ToList();

            if (relevantBrackets.Any())
            {
                // Find the highest bracket reached
                var activeBracket = relevantBrackets
                    .Where(b => taxableIncome >= b.LowerBound)
                    .OrderByDescending(b => b.LowerBound)
                    .FirstOrDefault();

                if (activeBracket != null)
                {
                    // Formula: BaseTax + (TaxableIncome - LowerBound) * Rate
                    annualTax = activeBracket.BaseTax + (taxableIncome - activeBracket.LowerBound) * activeBracket.Rate;
                }
            }

            // 4. Convert back to period and adjust for credits (W-4 Step 3)
            decimal periodFedTax = (annualTax / periodsInYear) - (profile.DependentsAmount / periodsInYear);
            if (periodFedTax < 0) periodFedTax = 0;

            // 5. Add Extra Withholding
            result.FederalTax = periodFedTax + profile.ExtraWithholdingAmount;

            return result;
        }
    }
}

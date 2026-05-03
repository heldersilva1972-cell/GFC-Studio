// [NEW]
using System.Collections.Generic;

namespace GFC.Core.Models.FormBuilder
{
    public class FormFieldDefinition
    {
        public string Name { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool Required { get; set; }
        public int? GridCols { get; set; }
        public string Placeholder { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new();
        public int? Rows { get; set; }
        public FormFieldValidation Validation { get; set; } = new();
        public FormFieldCondition Conditional { get; set; } = new();
        public string Description { get; set; } = string.Empty;
    }

    public class FormFieldValidation
    {
        public int? Max { get; set; }
        public int? Min { get; set; }
        public string Pattern { get; set; } = string.Empty;
    }

    public class FormFieldCondition
    {
        public string Field { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}

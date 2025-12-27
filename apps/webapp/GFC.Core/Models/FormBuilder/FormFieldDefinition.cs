// [NEW]
using System.Collections.Generic;

namespace GFC.Core.Models.FormBuilder
{
    public class FormFieldDefinition
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string Type { get; set; }
        public bool Required { get; set; }
        public int? GridCols { get; set; }
        public string Placeholder { get; set; }
        public List<string> Options { get; set; }
        public int? Rows { get; set; }
        public FormFieldValidation Validation { get; set; }
        public FormFieldCondition Conditional { get; set; }
        public string Description { get; set; }
    }

    public class FormFieldValidation
    {
        public int? Max { get; set; }
        public int? Min { get; set; }
        public string Pattern { get; set; }
    }

    public class FormFieldCondition
    {
        public string Field { get; set; }
        public string Value { get; set; }
    }
}

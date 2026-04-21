// [NEW]
using System.Collections.Generic;

namespace GFC.Core.Models.FormBuilder
{
    public class FormSection
    {
        public string Section { get; set; }
        public List<FormFieldDefinition> Fields { get; set; } = new List<FormFieldDefinition>();
    }
}

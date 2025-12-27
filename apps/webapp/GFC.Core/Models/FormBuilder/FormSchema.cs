// [NEW]
using System.Collections.Generic;

namespace GFC.Core.Models.FormBuilder
{
    public class FormSchema
    {
        public string Name { get; set; }
        public List<FormSection> Fields { get; set; } = new List<FormSection>();
    }
}

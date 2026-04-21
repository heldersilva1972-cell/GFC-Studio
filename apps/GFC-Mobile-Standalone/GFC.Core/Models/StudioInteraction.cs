using System;
using System.Collections.Generic;

namespace GFC.Core.Models
{
    public class StudioInteraction
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Trigger { get; set; } // e.g., "click", "hover", "scrollIntoView"
        public string Action { get; set; } // e.g., "navigate", "scrollTo", "toggleVisibility", "animate"
        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
    }
}

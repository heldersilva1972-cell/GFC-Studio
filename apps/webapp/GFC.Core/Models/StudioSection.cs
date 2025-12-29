using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text.Json;

namespace GFC.Core.Models
{
    public class StudioSection
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudioPageId { get; set; }

        [ForeignKey("StudioPageId")]
        public virtual StudioPage StudioPage { get; set; }

        [Required]
        [StringLength(100)]
        public string ComponentType { get; set; } = "TextBlock";

        public int OrderIndex { get; set; } = 0;

        [Required]
        public string Data { get; set; } = "{}";

        public string? AnimationSettingsJson { get; set; }
        
        [Column("Styles")]
        public string? StylesJson { get; set; } = "{}";

        public string? InteractionJson { get; set; } = "[]";
        
        [NotMapped]
        public List<StudioInteraction> Interactions { get; set; } = new List<StudioInteraction>();

        // --- Metadata ---
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(100)]
        public string CreatedBy { get; set; } = "System";

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(100)]
        public string UpdatedBy { get; set; } = "System";

        // --- Visibility Controls ---
        public bool IsVisible { get; set; } = true;
        public bool VisibleOnDesktop { get; set; } = true;
        public bool VisibleOnTablet { get; set; } = true;
        public bool VisibleOnMobile { get; set; } = true;

        // --- Frontend / UI Compatibility Properties ---

        [NotMapped]
        public string? AnimationSettings 
        { 
            get => AnimationSettingsJson; 
            set => AnimationSettingsJson = value; 
        }

        [NotMapped]
        public Guid ClientId { get; set; } = Guid.NewGuid();

        [NotMapped]
        public string Type 
        { 
            get => ComponentType; 
            set => ComponentType = value; 
        }

        [NotMapped]
        public string sectionType 
        { 
            get => ComponentType; 
            set => ComponentType = value; 
        }

        [NotMapped]
        public string PropertiesJson 
        { 
            get => Data; 
            set => Data = value; 
        }

        [NotMapped]
        public string Title { get; set; } // Display title for the section

        [NotMapped]
        public string Content { get; set; } // For RichTextBlock content

        [NotMapped]
        public int PageIndex 
        { 
            get => OrderIndex; 
            set => OrderIndex = value; 
        }

        [NotMapped]
        public Dictionary<string, object> properties { get; set; } = new Dictionary<string, object>();

        [NotMapped]
        public Dictionary<string, string> Styles { get; set; } = new Dictionary<string, string>();

        // Helpers to sync properties <-> Data/Content JSON
        public void SyncPropertiesToData()
        {
            // If Content is set, ensure it's in properties
            if (!string.IsNullOrEmpty(Content))
            {
                properties["content"] = Content;
            }
            // Serialize properties to Data
            Data = JsonSerializer.Serialize(properties);
            // Serialize Styles to StylesJson
            StylesJson = JsonSerializer.Serialize(Styles);
        }

        public void SyncDataToProperties()
        {
            if (!string.IsNullOrEmpty(Data))
            {
                try 
                {
                    properties = JsonSerializer.Deserialize<Dictionary<string, object>>(Data) ?? new Dictionary<string, object>();
                    // If content is in properties, pull it out to Content
                    if (properties.ContainsKey("content"))
                    {
                        Content = properties["content"].ToString();
                    }
                    if (properties.ContainsKey("headline"))
                    {
                        Title = properties["headline"].ToString();
                    }
                }
                catch { /* ignore serialization errors */ }
            }
            
            if (!string.IsNullOrEmpty(StylesJson))
            {
                try
                {
                    Styles = JsonSerializer.Deserialize<Dictionary<string, string>>(StylesJson) ?? new Dictionary<string, string>();
                }
                catch { /* ignore styles serialization errors */ }
            }

            if (!string.IsNullOrEmpty(InteractionJson))
            {
                try
                {
                    Interactions = JsonSerializer.Deserialize<List<StudioInteraction>>(InteractionJson) ?? new List<StudioInteraction>();
                }
                catch { /* ignore interaction serialization errors */ }
            }

            if (string.IsNullOrEmpty(Title)) Title = ComponentType;
        }

        public void SyncInteractionsToData()
        {
             InteractionJson = JsonSerializer.Serialize(Interactions);
        }
    }
}

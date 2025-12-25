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
        public StudioPage StudioPage { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [Required]
        public int OrderIndex { get; set; }

        // Backing field for general content/properties
        public string Data { get; set; }
        
        public string AnimationSettingsJson { get; set; }
        feature/content-ingestion-engine-5389181148924895155
        public string ComponentType { get; set; } = "TextBlock";

        public string? AnimationSettings { get; set; }

        // --- Frontend / UI Compatibility Properties ---
        master

        [NotMapped]
        public string AnimationSettings 
        { 
            get => AnimationSettingsJson; 
            set => AnimationSettingsJson = value; 
        }

        [NotMapped]
        public Guid ClientId { get; set; } = Guid.NewGuid();

        [NotMapped]
        public string sectionType 
        { 
            get => Type; 
            set => Type = value; 
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
            if (string.IsNullOrEmpty(Title)) Title = Type;
        }
    }
}

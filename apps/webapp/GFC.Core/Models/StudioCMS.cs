using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class StudioCollection
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Slug { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<StudioCollectionField> Fields { get; set; } = new List<StudioCollectionField>();
        public virtual ICollection<StudioCollectionItem> Items { get; set; } = new List<StudioCollectionItem>();
    }

    public class StudioCollectionField
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CollectionId { get; set; }

        [ForeignKey("CollectionId")]
        public virtual StudioCollection Collection { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Key { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Type { get; set; } = "Text"; // Text, RichText, Image, Date, Number

        public bool IsRequired { get; set; } = false;
    }

    public class StudioCollectionItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CollectionId { get; set; }

        [ForeignKey("CollectionId")]
        public virtual StudioCollection Collection { get; set; }

        [Required]
        public string DataJson { get; set; } = "{}";

        public bool IsPublished { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
    }
}

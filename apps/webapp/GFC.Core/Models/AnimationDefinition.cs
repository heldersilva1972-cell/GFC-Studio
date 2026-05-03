// [NEW]
using System.Text.Json.Serialization;

namespace GFC.Core.Models
{
    public class AnimationDefinition
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new();

        [JsonPropertyName("keyframes")]
        public List<AnimationKeyframe> Keyframes { get; set; } = new();
    }
}

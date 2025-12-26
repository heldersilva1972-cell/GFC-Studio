// [NEW]
using System.Text.Json.Serialization;

namespace GFC.Core.Models
{
    public class AnimationDefinition
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; }

        [JsonPropertyName("keyframes")]
        public List<AnimationKeyframe> Keyframes { get; set; }
    }
}

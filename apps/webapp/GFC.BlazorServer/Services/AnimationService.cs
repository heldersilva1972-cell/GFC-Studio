// [NEW]
using GFC.Core.Interfaces;
using GFC.Core.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GFC.BlazorServer.Services
{
    public class AnimationService : IAnimationService
    {
        public async Task<AnimationDefinition> GetAnimationByIdAsync(string id)
        {
            // This is a placeholder implementation. In a real application, this would fetch data from a database.
            if (id == "home-hero")
            {
                return await Task.FromResult(new AnimationDefinition
                {
                    Id = "home-hero",
                    Name = "Official Home Page Hero",
                    Description = "The main hero animation for the website's home page.",
                    Tags = new List<string> { "hero", "homepage" },
                    Keyframes = new List<AnimationKeyframe>
                    {
                        new AnimationKeyframe { Target = "headline", Effect = "slide-in-up", Duration = 0.8, Delay = 0.2, Easing = "ease-out" },
                        new AnimationKeyframe { Target = "subtitle", Effect = "slide-in-up", Duration = 0.8, Delay = 0.4, Easing = "ease-out" },
                        new AnimationKeyframe { Target = "primary-cta", Effect = "fade-in", Duration = 0.5, Delay = 0.8, Easing = "ease-in" },
                        new AnimationKeyframe { Target = "secondary-cta", Effect = "fade-in", Duration = 0.5, Delay = 1.0, Easing = "ease-in" },
                        new AnimationKeyframe { Target = "background-image", Effect = "scale-in", Duration = 1.5, Delay = 0, Easing = "ease-out-expo" }
                    }
                });
            }
            if (id == "hall-rental-showcase")
            {
                return await Task.FromResult(new AnimationDefinition
                {
                    Id = "hall-rental-showcase",
                    Name = "Hall Rental Showcase",
                    Description = "A showcase of the hall for rent.",
                    Tags = new List<string> { "hero", "hall-rental" },
                    Keyframes = new List<AnimationKeyframe>
                    {
                        new AnimationKeyframe { Target = "headline", Effect = "fade-in", Duration = 1.0, Delay = 0.2, Easing = "ease-in-out" },
                        new AnimationKeyframe { Target = "subtitle", Effect = "fade-in", Duration = 1.0, Delay = 0.5, Easing = "ease-in-out" },
                        new AnimationKeyframe { Target = "background-image", Effect = "scale-in", Duration = 1.5, Delay = 0, Easing = "ease-out-expo" }
                    }
                });
            }
            return null;
        }
    }
}

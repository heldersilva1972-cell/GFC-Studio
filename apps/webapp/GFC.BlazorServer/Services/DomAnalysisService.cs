// [NEW]
using AngleSharp.Dom;
using GFC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GFC.BlazorServer.Services
{
    public class DomAnalysisService
    {
        public List<StudioSection> Analyze(IElement body)
        {
            var sections = new List<StudioSection>();
            FindSections(body, sections);
            return sections;
        }

        private void FindSections(IElement element, ICollection<StudioSection> sections)
        {
            foreach (var child in element.Children)
            {
                if (IsSection(child))
                {
                    sections.Add(CreateSection(child));
                }
                else
                {
                    // If the child is not a section, continue traversing its children
                    FindSections(child, sections);
                }
            }
        }

        private bool IsSection(IElement element)
        {
            var tagName = element.TagName.ToLower();
            var id = element.Id?.ToLower() ?? "";
            var classList = string.Join(" ", element.ClassList).ToLower();

            // Prioritize semantic tags
            if (new[] { "header", "footer", "nav", "main", "article", "section", "aside" }.Contains(tagName))
            {
                return true;
            }

            // Check for common ARIA roles
            var role = element.GetAttribute("role")?.ToLower();
            if (new[] { "banner", "contentinfo", "navigation", "main" }.Contains(role))
            {
                return true;
            }

            // Check for common ID and class names that indicate a major section
            if (id.Contains("header") || id.Contains("footer") || id.Contains("main") || id.Contains("content"))
            {
                return true;
            }
            if (classList.Contains("hero") || classList.Contains("feature") || classList.Contains("container"))
            {
                // Avoid matching on simple layout classes by checking for child elements
                return element.Children.Length > 0;
            }

            return false;
        }

        private StudioSection CreateSection(IElement element)
        {
            var tagName = element.TagName.ToLower();
            var id = element.Id?.ToLower() ?? "";
            var classList = string.Join(" ", element.ClassList).ToLower();

            string componentName = "RawHtml"; // Default

            // Map semantic tags to component names
            if (tagName == "header" || id.Contains("header") || element.GetAttribute("role") == "banner")
            {
                componentName = "Header";
            }
            else if (tagName == "footer" || id.Contains("footer") || element.GetAttribute("role") == "contentinfo")
            {
                componentName = "Footer";
            }
            else if (tagName == "nav" || element.GetAttribute("role") == "navigation")
            {
                componentName = "Navigation";
            }
            else if (tagName == "main" || id.Contains("main") || element.GetAttribute("role") == "main")
            {
                componentName = "MainContent";
            }
            else if (classList.Contains("hero"))
            {
                componentName = "HeroSection";
            }
            else if (tagName == "section" || tagName == "article")
            {
                componentName = "Section";
            }

            return new StudioSection
            {
                ComponentName = componentName,
                Content = element.OuterHtml,
                Order = 0 // Will be set by the caller
            };
        }
    }
}

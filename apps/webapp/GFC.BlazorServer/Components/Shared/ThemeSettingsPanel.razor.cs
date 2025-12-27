// [NEW]
using GFC.Core.Models;
using Microsoft.AspNetCore.Components;

namespace GFC.BlazorServer.Components.Shared
{
    public partial class ThemeSettingsPanel
    {
        [Parameter]
        public WebsiteSettings WebsiteSettings { get; set; } = new();

        [Parameter]
        public EventCallback<WebsiteSettings> OnSettingsChanged { get; set; }

        private async Task SaveChanges()
        {
            await OnSettingsChanged.InvokeAsync(WebsiteSettings);
        }
    }
}

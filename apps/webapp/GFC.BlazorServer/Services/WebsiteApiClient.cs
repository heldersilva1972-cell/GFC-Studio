// [NEW]
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace GFC.BlazorServer.Services
{
    public class WebsiteApiClient : IWebsiteApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _revalidateUrl;

        public WebsiteApiClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _revalidateUrl = configuration["WebsiteApi:RevalidateUrl"];
        }

        public async Task TriggerRevalidation(string tag)
        {
            var payload = new { tag };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            await _httpClient.PostAsync(_revalidateUrl, content);
        }
    }
}

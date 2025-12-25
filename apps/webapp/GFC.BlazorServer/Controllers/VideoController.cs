// [NEW]
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/video")]
    public class VideoController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _videoAgentBaseUrl;

        public VideoController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            // This URL should be the *actual* internal URL of the Video Agent, not the proxied one.
            _videoAgentBaseUrl = configuration["VideoAgent:DirectUrl"] ?? "http://localhost:8888";
        }

        [HttpGet("stream/{**path}")]
        public async Task<IActionResult> StreamProxy(string path)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var queryString = HttpContext.Request.QueryString.Value;
            var requestUrl = $"{_videoAgentBaseUrl}/live/{path}{queryString}";

            var response = await httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);

            if (!response.IsSuccessStatusCode)
            {
                return new StatusCodeResult((int)response.StatusCode);
            }

            var stream = await response.Content.ReadAsStreamAsync();

            // It's important to copy the content type header from the Video Agent's response.
            // For HLS, this will typically be "application/vnd.apple.mpegurl" for .m3u8 files
            // and "video/mp2t" for .ts segments.
            Response.ContentType = response.Content.Headers.ContentType?.ToString();

            return new FileStreamResult(stream, Response.ContentType);
        }
    }
}

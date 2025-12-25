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
            
            // Clean up the path to ensure it matches the Video Agent's expected structure.
            // The Video Agent serves HLS streams under the '/stream' request path.
            // If the incoming path is 'live/6/index.m3u8', we want to proxy to 'stream/6/index.m3u8'.
            string targetPath = path;
            if (path.StartsWith("live/"))
            {
                targetPath = path.Replace("live/", "stream/");
            }
            else if (!path.StartsWith("stream/"))
            {
                targetPath = "stream/" + path;
            }

            var requestUrl = $"{_videoAgentBaseUrl}/{targetPath}{queryString}";

            try
            {
                var response = await httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);

                if (!response.IsSuccessStatusCode)
                {
                    return new StatusCodeResult((int)response.StatusCode);
                }

                var stream = await response.Content.ReadAsStreamAsync();

                // It's important to copy the content type header from the Video Agent's response.
                Response.ContentType = response.Content.Headers.ContentType?.ToString();

                return new FileStreamResult(stream, Response.ContentType);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Proxy error: {ex.Message}");
            }
        }
    }
}

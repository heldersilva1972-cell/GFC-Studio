var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Serve static files from the website/public folder
var publicPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "website", "public"));

// Ensure the directory exists
if (!Directory.Exists(publicPath))
{
    Console.WriteLine($"ERROR: Directory not found: {publicPath}");
    // Fallback if running from different dir
    publicPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "website", "public")); 
}

Console.WriteLine($"Serving content from: {publicPath}");

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(publicPath),
    RequestPath = ""
});

app.UseDefaultFiles(new DefaultFilesOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(publicPath),
    RequestPath = ""
});

app.MapGet("/", async context =>
{
    var indexPath = Path.Combine(publicPath, "index.html");
    if (File.Exists(indexPath))
        await context.Response.SendFileAsync(indexPath);
    else
        await context.Response.WriteAsync("Error: index.html not found at " + indexPath);
});

// LISTEN ON PORT 8888
app.Run("http://localhost:8888");

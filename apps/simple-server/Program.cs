var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Serve static files from the website/public folder
var publicPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "website", "public"));

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(publicPath),
    RequestPath = ""
});

// Also serve default files (index.html)
app.UseDefaultFiles(new DefaultFilesOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(publicPath),
    RequestPath = ""
});

app.MapGet("/", async context =>
{
    await context.Response.SendFileAsync(Path.Combine(publicPath, "index.html"));
});

Console.WriteLine($"Serving GFC Website from: {publicPath}");
Console.WriteLine("Server running on http://localhost:3000");

app.Run("http://localhost:3000");

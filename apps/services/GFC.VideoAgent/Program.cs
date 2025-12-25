// [MODIFIED]
using GFC.VideoAgent.Services;
using Microsoft.AspNetCore.Http;
using GFC.VideoAgent.Middleware;
using GFC.Core.Interfaces;
using GFC.Core.Services;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<StreamManager>();
        services.AddSingleton<FFmpegService>();
        services.AddSingleton<NvrService>();
        services.AddSingleton<IStreamSecurityService, StreamSecurityService>();
        services.AddHostedService<StreamManager>();

        services.AddCors(options =>
        {
            options.AddPolicy("WebAppPolicy",
                builder =>
                {
                    var allowedOrigins = Configuration.GetSection("VideoAgent:AllowedOrigins").Get<string[]>();
                    if (allowedOrigins != null && allowedOrigins.Length > 0)
                    {
                        builder.WithOrigins(allowedOrigins)
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    }
                });
        });

        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseCors("WebAppPolicy");
        var outputDirectory = Configuration.GetValue<string>("VideoAgent:OutputDirectory") ?? "hls-streams";
        if (!Path.IsPathRooted(outputDirectory))
        {
            outputDirectory = Path.Combine(env.ContentRootPath, outputDirectory);
        }

        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        // Token validation must come before serving the static files.
        app.UseMiddleware<StreamTokenValidationMiddleware>();

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(outputDirectory),
            RequestPath = "/stream"
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", context => context.Response.WriteAsync("GFC Video Agent is running."));

            // [NEW] Health check endpoint
            endpoints.MapGet("/health", context =>
            {
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsJsonAsync(new { status = "healthy", timestamp = DateTime.UtcNow });
            });

            // [NEW] Stream status endpoint
            endpoints.MapGet("/stream/{cameraId}/status", async context =>
            {
                if (!int.TryParse(context.Request.RouteValues["cameraId"]?.ToString(), out var cameraId))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new { message = "Invalid camera ID format." });
                    return;
                }

                var streamManager = context.Request.Services.GetRequiredService<StreamManager>();
                var status = streamManager.GetStreamStatus(cameraId);

                // Check if the stream status was found for the given camera ID
                if (status == null)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsJsonAsync(new { message = $"Status for camera {cameraId} not found." });
                    return;
                }

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { cameraId, status = status.ToString(), timestamp = DateTime.UtcNow });
            });

            endpoints.MapControllers();
        });
    }
}

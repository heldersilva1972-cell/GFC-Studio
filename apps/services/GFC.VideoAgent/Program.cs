// [NEW]
using GFC.VideoAgent.Services;

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

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(outputDirectory),
            RequestPath = "/stream"
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("GFC Video Agent is running.");
            });
        });
    }
}

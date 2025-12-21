using Gfc.Agent.Api.Auth;
using Gfc.Agent.Api.Configuration;
using Gfc.Agent.Api.Endpoints;
using Gfc.Agent.Api.Services;
using Gfc.ControllerClient.Abstractions;
using Gfc.ControllerClient.Configuration;
using Gfc.ControllerClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AgentApiOptions>(builder.Configuration.GetSection("AgentApi"));
builder.Services.AddSingleton<ApiRequestExecutor>();
builder.Services.AddSingleton<IControllerEndpointResolver, AppSettingsControllerEndpointResolver>();
builder.Services.AddSingleton<ControllerClientOptions>(_ => ControllerClientOptionsFactory.Create(builder.Configuration));
builder.Services.AddSingleton<IMengqiControllerClient>(serviceProvider =>
{
    var options = serviceProvider.GetRequiredService<ControllerClientOptions>();
    var resolver = serviceProvider.GetRequiredService<IControllerEndpointResolver>();
    var logger = serviceProvider.GetRequiredService<ILogger<MengqiControllerClient>>();
    return new MengqiControllerClient(options, resolver, transport: null, logger);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ApiKeyAuthenticationMiddleware>();

app.MapControllerEndpoints();

app.Run();


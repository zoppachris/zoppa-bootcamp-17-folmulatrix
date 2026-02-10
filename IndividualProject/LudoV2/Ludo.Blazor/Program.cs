using Ludo.Blazor.Components;
using Ludo.Game.Logging;
using Ludo.Game.Controller;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.File;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .Enrich.WithEnvironmentName()
        .Enrich.WithMachineName()
        .WriteTo.Console()
        .WriteTo.File("logs/application-.log",
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 30,
            shared: true,
            flushToDiskInterval: TimeSpan.FromSeconds(1))
        .WriteTo.File(new CompactJsonFormatter(), "logs/application-json-.log",
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 30));

    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();
    builder.Services.AddScoped<SoundService>();

    builder.Services.AddScoped<GameSessionService>();
    builder.Services.AddScoped<GameControllerFactory>();
    builder.Services.AddScoped<IGameLogger, SerilogGameLogger>();

    builder.Services.AddBlazorBootstrap();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    // app.UseHttpsRedirection();


    app.UseAntiforgery();

    app.MapStaticAssets();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    app.Run();
}
catch (System.Exception)
{

    throw;
}

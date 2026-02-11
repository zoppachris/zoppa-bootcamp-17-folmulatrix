using Ludo.Blazor.Components;
using Ludo.Game.Logging;
using Ludo.Game.Controller;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

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

    var solutionRoot = Directory.GetParent(
        builder.Environment.ContentRootPath
    )!.FullName;

    var logRoot = Path.Combine(
        solutionRoot,
        "logs"
    );

    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithEnvironmentName()
        .Enrich.WithMachineName()
        .WriteTo.Console();

        if (!context.HostingEnvironment.IsDevelopment())
        {
            configuration.ReadFrom.Services(services);
        }
    
        configuration.WriteTo.Async(a =>
        {
            a.File(
                Path.Combine(logRoot, "application-.log"),
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30,
                shared: true,
                flushToDiskInterval: TimeSpan.FromSeconds(1));

            // a.File(
            //     new CompactJsonFormatter(),
            //     Path.Combine(logRoot, "application-json-.log"),
            //     rollingInterval: RollingInterval.Day,
            //     retainedFileCountLimit: 30,
            //     shared: true);
        });
    });

    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();
    builder.Services.AddScoped<SoundService>();

    builder.Services.AddScoped<GameSessionService>();
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
finally
{
    Log.CloseAndFlush();
}

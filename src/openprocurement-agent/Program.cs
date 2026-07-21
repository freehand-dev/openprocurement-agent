using Microsoft.EntityFrameworkCore;
using openprocurement.api.client;
using openprocurement_agent.Extensions;
using openprocurement_agent.Models;
using openprocurement_agent.Services;
using System.Reflection;
using System.Security.Authentication;

var builder = WebApplication.CreateBuilder(args);

#region ConfigureAppConfiguration
var appName = builder.Environment.ApplicationName;
var env = builder.Environment.EnvironmentName;
Console.WriteLine($"Loading configuration for application {appName} in environment {env}...");
builder.Configuration.AddJsonFile($"{appName}.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile($"{appName}.{env}.json", optional: true, reloadOnChange: true);
builder.Configuration.AddYamlFile($"{appName}.yaml", optional: true, reloadOnChange: true);
builder.Configuration.AddYamlFile($"{appName}.{env}.yaml", optional: true, reloadOnChange: true);
builder.Configuration.AddIniFile($"{appName}.conf", optional: true, reloadOnChange: true);
builder.Configuration.AddIniFile($"{appName}.{env}.conf", optional: true, reloadOnChange: true);
builder.Configuration.AddCommandLine(args);
builder.Configuration.AddEnvironmentVariables();
#endregion

#region ConfigureLogging
builder.Logging.AddConfiguration((IConfiguration)builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();
#endregion

// Log application startup info after logging is configured
var loggerFactory = LoggerFactory.Create(loggingBuilder =>
{
    loggingBuilder.AddConfiguration(builder.Configuration.GetSection("Logging"));
    loggingBuilder.AddConsole();
});
var startupLogger = loggerFactory.CreateLogger<Program>();
startupLogger.LogInformation("starting application with environment {Environment}", builder.Environment.EnvironmentName);
startupLogger.LogDebug("content root path: {ContentRoot}", builder.Environment.ContentRootPath);

#region ConfigureWebHostDefaults
var webBuilder = builder.WebHost;
webBuilder.UseKestrel((context, serverOptions) =>
{
    serverOptions.Configure((IConfiguration)context.Configuration.GetSection("Kestrel"))
        .Endpoint("HTTPS", listenOptions =>
        {
            listenOptions.HttpsOptions.SslProtocols = SslProtocols.Tls12;
        });
});
#endregion

// Configuration
builder.Services.Configure<AppSettings>(builder.Configuration);

// Add services to the container.
builder.Services.ConfigureEntityFramework(builder.Configuration);
builder.Services.ConfigureDataProtection(builder.Configuration, startupLogger);
builder.Services.AddCors();
builder.Services.ConfigureCookie();

//
// Add HttpClient services
builder.Services.AddHttpClient();

// Register OpenprocurementClient API client service
builder.Services.AddHttpClient<IOpenprocurementClient, OpenprocurementClient>();

//OpenProcurement Service
builder.Services.AddHostedService<OpenprocurementService>();

// Add services to the container.
builder.Services.AddRazorPages();

// Health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<TenderHistoryDbContext>()
    .AddDbContextCheck<ProcuringEntityDbContext>()
    .AddDbContextCheck<PipelineSettingsDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();
app.MapHealthChecks("/health");

app.Run();

partial class Program
{
    public static string InstanceName = "Default";
    public static readonly string ServiceName = "openprocurement-agent";

    /// <summary>
    /// Gets current version of the application.
    /// It's also shown in the web page.
    /// </summary>
    public const string Version = "1.0.0.0";

    /// <summary>
    /// Gets release (last build) date of the application.
    /// It's shown in the web page.
    /// </summary>
    public static DateTime ReleaseDate => LzyReleaseDate.Value;

    private static readonly Lazy<DateTime> LzyReleaseDate = new Lazy<DateTime>(() => new FileInfo(typeof(Program).Assembly.Location).LastWriteTime);

    public static void PrintProductVersion()
    {
        var assembly = typeof(Program).Assembly;
        var product = assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product;
        var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Starting {product} v{version}...");
        Console.ResetColor();
    }
}

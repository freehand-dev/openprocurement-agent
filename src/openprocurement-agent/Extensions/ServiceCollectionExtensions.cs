using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace openprocurement_agent.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureEntityFramework(this IServiceCollection services, IConfiguration configuration)
        {
            // Add TenderHistoryDbContext.
            var tenderHistoryConnectionStringBuilder = new SqliteConnectionStringBuilder(configuration.GetConnectionString("TenderHistoryConnection"));
            services.AddDbContext<Models.TenderHistoryDbContext>(options =>
                    options.UseSqlite(tenderHistoryConnectionStringBuilder.ConnectionString,
                x => x.MigrationsAssembly(typeof(Models.TenderHistoryDbContext).Assembly.FullName)));

            // Add ProcuringEntityConnection
            var procuringEntityConnectionStringBuilder = new SqliteConnectionStringBuilder(configuration.GetConnectionString("ProcuringEntityConnection"));
            services.AddDbContext<Models.ProcuringEntityDbContext>(options =>
                    options.UseSqlite(procuringEntityConnectionStringBuilder.ConnectionString,
                x => x.MigrationsAssembly(typeof(Models.ProcuringEntityDbContext).Assembly.FullName)));

            // Register PipelineSettingsDbContext (one table per pipeline element: Global, each Transform and each Action)
            var settingsConnectionStringBuilder = new SqliteConnectionStringBuilder(configuration.GetConnectionString("SettingsConnection"));
            services.AddDbContext<Models.PipelineSettingsDbContext>(options =>
                    options.UseSqlite(settingsConnectionStringBuilder.ConnectionString,
                x => x.MigrationsAssembly(typeof(Models.PipelineSettingsDbContext).Assembly.FullName)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureCors(this IServiceCollection services, params string[]? origins)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowAny",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());

                if (origins != null)
                {
                    options.AddPolicy(name: "AllowSite",
                        builder => builder
                            .WithOrigins(origins)
                            .SetIsOriginAllowedToAllowWildcardSubdomains()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials());
                }
            });
        }

        public static void ConfigureCookie(this IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;

                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.SlidingExpiration = true;

                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";

            });
        }


        /// <summary>
        /// Configures ASP.NET Core Data Protection for key persistence.
        /// If DataProtection:KeyPath is configured, keys are persisted to that location.
        /// Otherwise, the default ASP.NET Core behavior is used (platform-specific).
        /// </summary>
        public static void ConfigureDataProtection(this IServiceCollection services, IConfiguration configuration, ILogger? logger = null)
        {
            var dataProtectionBuilder = services.AddDataProtection()
                .SetApplicationName(Program.ServiceName);

            var keyPath = configuration.GetValue<string>("DataProtection:KeyPath");

            if (!string.IsNullOrWhiteSpace(keyPath))
            {
                try
                {
                    // Ensure directory exists
                    var directory = new DirectoryInfo(keyPath);
                    if (!directory.Exists)
                    {
                        directory.Create();
                        logger?.LogInformation("created data protection keys directory at {KeyPath}", keyPath);
                    }

                    dataProtectionBuilder.PersistKeysToFileSystem(directory);
                    logger?.LogInformation("data protection keys will be persisted to {KeyPath}", keyPath);
                }
                catch (Exception ex)
                {
                    logger?.LogError(ex, "failed to configure data protection key path {KeyPath}, using default behavior", keyPath);
                    // Continue with default behavior if path configuration fails
                }
            }
            else
            {
                logger?.LogInformation("data protection key path not configured, using platform default behavior");
                // Default behavior:
                // - Windows: Keys stored in registry or user profile
                // - Linux: Keys stored in ~/.aspnet/DataProtection-Keys/
                // - Docker: Keys stored in container (lost on restart without volume)
                // For production Docker deployments, set DataProtection:KeyPath to a volume-mounted path
            }
        }

    }
}

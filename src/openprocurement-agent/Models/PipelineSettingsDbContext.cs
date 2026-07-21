using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace openprocurement_agent.Models
{
    /// <summary>
    /// Stores settings for the whole tender processing pipeline. Each pipeline
    /// element (Global options, every Transform filter and every Action) has
    /// its own table, so new pipeline elements can be added independently in
    /// the future without touching unrelated settings.
    /// </summary>
    public class PipelineSettingsDbContext : DbContext
    {
        /// <summary>[Global]</summary>
        public DbSet<GlobalSettings> GlobalSettings { get; set; }

        /// <summary>[Action:TendersHistory]</summary>
        public DbSet<TendersHistoryActionSettings> TendersHistoryActionSettings { get; set; }

        /// <summary>[Action:SendMail]</summary>
        public DbSet<MailSettings> MailSettings { get; set; }

        /// <summary>[Transform:TendersHistory]</summary>
        public DbSet<TendersHistoryTransformSettings> TendersHistoryTransformSettings { get; set; }

        /// <summary>[Transform:Status]</summary>
        public DbSet<StatusTransformSettings> StatusTransformSettings { get; set; }

        /// <summary>[Transform:Identifier]</summary>
        public DbSet<IdentifierTransformSettings> IdentifierTransformSettings { get; set; }

        /// <summary>[Transform:Classification]</summary>
        public DbSet<ClassificationTransformSettings> ClassificationTransformSettings { get; set; }

        public PipelineSettingsDbContext(DbContextOptions<PipelineSettingsDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlite(@"Data Source=Settings.db");
            }

            options.ConfigureWarnings(warnings => warnings
                .Ignore(RelationalEventId.PendingModelChangesWarning)
                // Every table here is a single-row (Id=1) settings table looked up via
                // DbSet.Find()/FindAsync(), so a missing OrderBy is never ambiguous.
                .Ignore(CoreEventId.FirstWithoutOrderByAndFilterWarning));
        }

        /// <summary>
        /// Seed default settings (matching original .conf values) for every pipeline
        /// element table that is still empty.
        /// </summary>
        public void SeedDefaults()
        {
            if (!GlobalSettings.Any())
            {
                GlobalSettings.Add(new GlobalSettings { Subtract = 1 });
            }

            if (!TendersHistoryActionSettings.Any())
            {
                TendersHistoryActionSettings.Add(new TendersHistoryActionSettings { Enabled = true });
            }

            if (!MailSettings.Any())
            {
                MailSettings.Add(new MailSettings
                {
                    Enabled = false,
                    From = "Tenders Agent <sender@corp-mail.com>",
                    Username = "sender@corp-mail.com",
                    Password = "",
                    Server = "smtp.server.com",
                    Port = 25,
                    EnableSsl = false,
                    Subject = "%Value.String% - %Title% - (%ProcuringEntity.Name%)",
                    MailTo = "user@corp-mail.com",
                    MessageTemplate = DefaultMessageTemplate
                });
            }

            if (!TendersHistoryTransformSettings.Any())
            {
                TendersHistoryTransformSettings.Add(new TendersHistoryTransformSettings { Enabled = true });
            }

            if (!StatusTransformSettings.Any())
            {
                StatusTransformSettings.Add(new StatusTransformSettings
                {
                    Enabled = true,
                    Allow = "active.enquiries\nactive.tendering"
                });
            }

            if (!IdentifierTransformSettings.Any())
            {
                IdentifierTransformSettings.Add(new IdentifierTransformSettings { Enabled = true });
            }

            if (!ClassificationTransformSettings.Any())
            {
                ClassificationTransformSettings.Add(new ClassificationTransformSettings
                {
                    Enabled = false,
                    Bypass = "",
                    Block = ""
                });
            }

            SaveChanges();
        }

        /// <summary>
        /// Parses a newline-separated list of values into a trimmed, non-empty HashSet.
        /// </summary>
        public static HashSet<string> ParseList(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return new HashSet<string>();

            return value
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s))
                .ToHashSet();
        }

        public const string DefaultMessageTemplate = @"<html>
	<head>
		<style>
			body, td{
				font-size: 14px;
				vertical-align: top;
			}
			table.border{
				border-collapse: collapse;
			}
			table.border td{
				border: 1px solid #888;
			}
			.small{
				font-size:12px;
			}
			h2{
				margin-bottom:0px;
			}
			center{
				text-align: center;
			}
		</style>
		<meta name=""robots"" content=""noindex"" />
	</head>
	<body>

		%body%

	</body>
</html>";
    }

    /// <summary>[Global] section: options shared by the whole pipeline.</summary>
    public class GlobalSettings
    {
        [Key]
        public int Id { get; set; } = 1;

        [Display(Name = "Зсув старту (годин)")]
        public int Subtract { get; set; } = 1;
    }

    /// <summary>[Action:TendersHistory]: stores the processed tender in the history DB.</summary>
    public class TendersHistoryActionSettings
    {
        [Key]
        public int Id { get; set; } = 1;

        [Display(Name = "Дія: Історія тендерів")]
        public bool Enabled { get; set; } = true;
    }

    /// <summary>[Transform:TendersHistory]: skips tenders already present in the history DB.</summary>
    public class TendersHistoryTransformSettings
    {
        [Key]
        public int Id { get; set; } = 1;

        [Display(Name = "Трансформація: Історія тендерів")]
        public bool Enabled { get; set; } = true;
    }

    /// <summary>[Transform:Status]: only allows tenders with a whitelisted status.</summary>
    public class StatusTransformSettings
    {
        [Key]
        public int Id { get; set; } = 1;

        [Display(Name = "Трансформація: Статус")]
        public bool Enabled { get; set; } = true;

        [Display(Name = "Дозволені статуси (по одному на рядок)")]
        public string Allow { get; set; } = "active.enquiries\nactive.tendering";
    }

    /// <summary>[Transform:Identifier]: only allows tenders from known Procuring Entities.</summary>
    public class IdentifierTransformSettings
    {
        [Key]
        public int Id { get; set; } = 1;

        [Display(Name = "Трансформація: Ідентифікатор")]
        public bool Enabled { get; set; } = true;
    }

    /// <summary>[Transform:Classification]: bypass/block tenders by CPV classification code.</summary>
    public class ClassificationTransformSettings
    {
        [Key]
        public int Id { get; set; } = 1;

        [Display(Name = "Трансформація: Класифікація")]
        public bool Enabled { get; set; } = true;

        [Display(Name = "Пропустити коди (по одному на рядок)")]
        public string Bypass { get; set; } = "";

        [Display(Name = "Заблоковані коди (по одному на рядок)")]
        public string Block { get; set; } = "";
    }

    /// <summary>
    /// Settings for the SendMail pipeline action (Action:SendMail).
    /// </summary>
    public class MailSettings
    {
        [Key]
        public int Id { get; set; } = 1;

        public bool Enabled { get; set; } = false;

        [Display(Name = "Відправник")]
        public string From { get; set; } = "Tenders Agent <sender@corp-mail.com>";

        [Display(Name = "Користувач (SMTP)")]
        public string Username { get; set; } = "";

        [Display(Name = "Пароль")]
        public string Password { get; set; } = "";

        [Display(Name = "Сервер SMTP")]
        public string Server { get; set; } = "smtp.server.com";

        [Display(Name = "Порт")]
        public int Port { get; set; } = 25;

        [Display(Name = "Увімкнути SSL")]
        public bool EnableSsl { get; set; } = false;

        [Display(Name = "Тема листа")]
        public string Subject { get; set; } = "%Value.String% - %Title% - (%ProcuringEntity.Name%)";

        [Display(Name = "Одержувачі (по одному на рядок)")]
        public string MailTo { get; set; } = "";

        [Display(Name = "Шаблон листа (HTML)")]
        public string MessageTemplate { get; set; } = "";
    }
}


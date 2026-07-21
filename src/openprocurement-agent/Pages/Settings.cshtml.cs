using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using openprocurement_agent.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace openprocurement_agent.Pages
{
    public class SettingsModel : PageModel
    {
        private readonly PipelineSettingsDbContext _pipelineDb;

        public SettingsModel(PipelineSettingsDbContext pipelineDb)
        {
            _pipelineDb = pipelineDb;
        }

        /// <summary>
        /// Known tender statuses (from <see cref="openprocurement.api.client.Models.Tender.StatusEnum"/>),
        /// shown as a checklist so the user knows which values are valid for the
        /// "allowed statuses" filter instead of having to guess free-text values.
        /// </summary>
        public static readonly IReadOnlyList<TenderStatusOption> KnownTenderStatuses = new List<TenderStatusOption>
        {
            new("active", "Активний (за замовчуванням)"),
            new("active.enquiries", "Період уточнень"),
            new("active.tendering", "Період подання пропозицій"),
            new("active.auction", "Період аукціону"),
            new("active.qualification", "Кваліфікація переможця"),
            new("active.pre-qualification", "Попередня кваліфікація"),
            new("active.pre-qualification.stand-still", "Очікування перед аукціоном"),
            new("active.awarded", "Період очікування (переможець визначений)"),
            new("unsuccessful", "Тендер не відбувся"),
            new("complete", "Завершено"),
            new("cancelled", "Скасовано"),
            new("draft", "Чернетка процедури"),
            new("draft.pending", "Чернетка на перевірці"),
            new("draft.unsuccessful", "Чернетка неуспішна"),
        };

        [BindProperty]
        public MailSettings Settings { get; set; } = new();

        /// <summary>
        /// Flat form model combining every pipeline element table for editing on
        /// a single form; each field is persisted to its own dedicated table.
        /// </summary>
        [BindProperty]
        public PipelineFormModel Pipeline { get; set; } = new();

        /// <summary>
        /// Checked values from the allowed-statuses checklist, bound directly from the
        /// posted checkbox inputs (name="SelectedStatuses"); no hidden field needed.
        /// </summary>
        [BindProperty]
        public List<string> SelectedStatuses { get; set; } = new();

        public async Task OnGetAsync()
        {
            // Ensure defaults are seeded
            _pipelineDb.SeedDefaults();

            var settings = await _pipelineDb.MailSettings.FindAsync(1);
            if (settings != null)
                Settings = settings;

            var global = await _pipelineDb.GlobalSettings.FindAsync(1);
            var tendersHistoryAction = await _pipelineDb.TendersHistoryActionSettings.FindAsync(1);
            var tendersHistoryTransform = await _pipelineDb.TendersHistoryTransformSettings.FindAsync(1);
            var status = await _pipelineDb.StatusTransformSettings.FindAsync(1);
            var identifier = await _pipelineDb.IdentifierTransformSettings.FindAsync(1);
            var classification = await _pipelineDb.ClassificationTransformSettings.FindAsync(1);

            Pipeline = new PipelineFormModel
            {
                Subtract = global?.Subtract ?? 1,
                ActionTendersHistoryEnabled = tendersHistoryAction?.Enabled ?? true,
                TransformTendersHistoryEnabled = tendersHistoryTransform?.Enabled ?? true,
                TransformStatusEnabled = status?.Enabled ?? true,
                TransformStatusAllow = status?.Allow ?? "",
                TransformIdentifierEnabled = identifier?.Enabled ?? true,
                TransformClassificationEnabled = classification?.Enabled ?? true,
                TransformClassificationBypass = classification?.Bypass ?? "",
                TransformClassificationBlock = classification?.Block ?? ""
            };

            SelectedStatuses = Pipeline.TransformStatusAllow
                .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList();
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            // Action:SendMail
            // The password input is always rendered empty (browsers never echo back
            // a stored password), so an empty submitted value means "keep the current
            // password" rather than "clear the password".
            var existing = await _pipelineDb.MailSettings.FindAsync(1);
            if (existing == null)
            {
                Settings.Password ??= "";
                _pipelineDb.MailSettings.Add(Settings);
            }
            else
            {
                existing.Enabled = Settings.Enabled;
                existing.From = Settings.From ?? "";
                existing.Username = Settings.Username ?? "";
                if (!string.IsNullOrEmpty(Settings.Password))
                    existing.Password = Settings.Password;
                existing.Server = Settings.Server ?? "";
                existing.Port = Settings.Port;
                existing.EnableSsl = Settings.EnableSsl;
                existing.Subject = Settings.Subject ?? "";
                existing.MailTo = Settings.MailTo ?? "";
                existing.MessageTemplate = Settings.MessageTemplate ?? "";
            }

            // Global
            var global = await _pipelineDb.GlobalSettings.FindAsync(1);
            if (global == null)
                _pipelineDb.GlobalSettings.Add(new GlobalSettings { Subtract = Pipeline.Subtract });
            else
                global.Subtract = Pipeline.Subtract;

            // Action:TendersHistory
            var tendersHistoryAction = await _pipelineDb.TendersHistoryActionSettings.FindAsync(1);
            if (tendersHistoryAction == null)
                _pipelineDb.TendersHistoryActionSettings.Add(new TendersHistoryActionSettings { Enabled = Pipeline.ActionTendersHistoryEnabled });
            else
                tendersHistoryAction.Enabled = Pipeline.ActionTendersHistoryEnabled;

            // Transform:TendersHistory
            var tendersHistoryTransform = await _pipelineDb.TendersHistoryTransformSettings.FindAsync(1);
            if (tendersHistoryTransform == null)
                _pipelineDb.TendersHistoryTransformSettings.Add(new TendersHistoryTransformSettings { Enabled = Pipeline.TransformTendersHistoryEnabled });
            else
                tendersHistoryTransform.Enabled = Pipeline.TransformTendersHistoryEnabled;

            // Transform:Status — rebuild the allowed-statuses string directly from the checked checkboxes.
            Pipeline.TransformStatusAllow = string.Join("\n", SelectedStatuses ?? new List<string>());
            var status = await _pipelineDb.StatusTransformSettings.FindAsync(1);
            if (status == null)
                _pipelineDb.StatusTransformSettings.Add(new StatusTransformSettings { Enabled = Pipeline.TransformStatusEnabled, Allow = Pipeline.TransformStatusAllow });
            else
            {
                status.Enabled = Pipeline.TransformStatusEnabled;
                status.Allow = Pipeline.TransformStatusAllow;
            }

            // Transform:Identifier
            var identifier = await _pipelineDb.IdentifierTransformSettings.FindAsync(1);
            if (identifier == null)
                _pipelineDb.IdentifierTransformSettings.Add(new IdentifierTransformSettings { Enabled = Pipeline.TransformIdentifierEnabled });
            else
                identifier.Enabled = Pipeline.TransformIdentifierEnabled;

            // Transform:Classification
            // Empty textareas are posted as "", but ASP.NET Core's default model binding
            // (ConvertEmptyStringToNull) turns that into null before it reaches this handler,
            // which would violate the NOT NULL constraint on Bypass/Block — so coalesce back to "".
            var classification = await _pipelineDb.ClassificationTransformSettings.FindAsync(1);
            if (classification == null)
                _pipelineDb.ClassificationTransformSettings.Add(new ClassificationTransformSettings
                {
                    Enabled = Pipeline.TransformClassificationEnabled,
                    Bypass = Pipeline.TransformClassificationBypass ?? "",
                    Block = Pipeline.TransformClassificationBlock ?? ""
                });
            else
            {
                classification.Enabled = Pipeline.TransformClassificationEnabled;
                classification.Bypass = Pipeline.TransformClassificationBypass ?? "";
                classification.Block = Pipeline.TransformClassificationBlock ?? "";
            }

            await _pipelineDb.SaveChangesAsync();
            TempData["SuccessMessage"] = "Налаштування збережено.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostResetTemplateAsync()
        {
            var existing = await _pipelineDb.MailSettings.FindAsync(1);
            if (existing != null)
            {
                existing.MessageTemplate = PipelineSettingsDbContext.DefaultMessageTemplate;
                await _pipelineDb.SaveChangesAsync();
                TempData["SuccessMessage"] = "Шаблон листа скинуто до значення за замовчуванням.";
            }
            return RedirectToPage();
        }
    }

    /// <summary>
    /// Flat editing model for the Pipeline tab, backed by the separate
    /// Global/Transform/Action tables in <see cref="PipelineSettingsDbContext"/>.
    /// </summary>
    public class PipelineFormModel
    {
        public int Subtract { get; set; } = 1;
        public bool ActionTendersHistoryEnabled { get; set; } = true;
        public bool TransformTendersHistoryEnabled { get; set; } = true;
        public bool TransformStatusEnabled { get; set; } = true;
        public string TransformStatusAllow { get; set; } = "";
        public bool TransformIdentifierEnabled { get; set; } = true;
        public bool TransformClassificationEnabled { get; set; } = true;
        public string TransformClassificationBypass { get; set; } = "";
        public string TransformClassificationBlock { get; set; } = "";
    }

    /// <summary>
    /// A known tender status value paired with a human-readable Ukrainian description,
    /// used to render the allowed-statuses checklist on the Settings page.
    /// </summary>
    public record TenderStatusOption(string Value, string Description);
}
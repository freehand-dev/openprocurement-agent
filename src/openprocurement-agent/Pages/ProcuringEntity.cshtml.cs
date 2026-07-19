using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using openprocurement_agent.Models;
using System.Globalization;
using System.Text;

namespace openprocurement_agent.Pages
{
    public class ProcuringEntityModel : PageModel
    {
        private readonly ProcuringEntityDbContext _db;
        private readonly ILogger<ProcuringEntityModel> _logger;

        public ProcuringEntityModel(ProcuringEntityDbContext db, ILogger<ProcuringEntityModel> logger)
        {
            _db = db;
            _logger = logger;
        }

        public List<ProcuringEntity> Entities { get; set; } = new();
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 25;
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        [BindProperty]
        public ProcuringEntity FormEntity { get; set; } = new() { Code = "" };

        [BindProperty(SupportsGet = true)]
        public string? SearchCode { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? PageNumber { get; set; }

        public async Task OnGetAsync()
        {
            await LoadEntitiesAsync();
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadEntitiesAsync();
                return Page();
            }

            var existing = await _db.ProcuringEntitys
                .FirstOrDefaultAsync(e => e.Code == FormEntity.Code);
            if (existing != null)
            {
                ModelState.AddModelError("FormEntity.Code", "Запис з таким кодом вже існує.");
                await LoadEntitiesAsync();
                return Page();
            }

            _db.ProcuringEntitys.Add(FormEntity);
            await _db.SaveChangesAsync();
            TempData["SuccessMessage"] = "Запис додано.";
            return RedirectToPage(new { SearchCode, PageNumber = CurrentPage });
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadEntitiesAsync();
                return Page();
            }

            var entity = await _db.ProcuringEntitys
                .FirstOrDefaultAsync(e => e.Code == FormEntity.Code);
            if (entity == null)
            {
                TempData["ErrorMessage"] = "Запис не знайдено.";
                return RedirectToPage();
            }

            entity.Name = FormEntity.Name;
            entity.LegalName = FormEntity.LegalName;
            entity.Addreess = FormEntity.Addreess;
            await _db.SaveChangesAsync();
            TempData["SuccessMessage"] = "Запис оновлено.";
            return RedirectToPage(new { SearchCode, PageNumber = CurrentPage });
        }

        public async Task<IActionResult> OnPostDeleteAsync(string code)
        {
            var entity = await _db.ProcuringEntitys
                .FirstOrDefaultAsync(e => e.Code == code);
            if (entity == null)
            {
                TempData["ErrorMessage"] = "Запис не знайдено.";
                return RedirectToPage();
            }

            _db.ProcuringEntitys.Remove(entity);
            await _db.SaveChangesAsync();
            TempData["SuccessMessage"] = "Запис видалено.";
            return RedirectToPage(new { SearchCode, PageNumber = CurrentPage });
        }

        public async Task<JsonResult> OnGetCheckCodeAsync(string code)
        {
            var exists = await _db.ProcuringEntitys.AnyAsync(e => e.Code == code);
            return new JsonResult(new { exists });
        }

        public async Task<IActionResult> OnPostImportCsvAsync(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                TempData["ErrorMessage"] = "Файл не вибрано або порожній.";
                return RedirectToPage();
            }

            int added = 0, skipped = 0;

            try
            {
                using var stream = csvFile.OpenReadStream();
                using var reader = new StreamReader(stream, detectEncodingFromByteOrderMarks: true);

                // Read header line
                var headerLine = await reader.ReadLineAsync();
                if (headerLine == null)
                {
                    TempData["ErrorMessage"] = "Файл порожній.";
                    return RedirectToPage();
                }

                // Strip BOM if present
                headerLine = headerLine.TrimStart('\uFEFF');

                var headers = ParseCsvLine(headerLine);

                // Flexible column mapping — supports multiple possible header names
                int idxCode = FindIndexFlexible(headers, new[] { "Код ЄДРПОУ/РНОКПП", "Код", "Code", "ЄДРПОУ", "РНОКПП", "EDRPOU", "code" });
                int idxLegalName = FindIndexFlexible(headers, new[] { "Повне найменування", "LegalName", "Юридична назва", "Юр. назва", "legalName", "legal_name" });
                int idxName = FindIndexFlexible(headers, new[] { "Назва", "Name", "Логотип/Назва медіа", "Назва медіа", "name", "short_name" });
                int idxCity = FindIndexFlexible(headers, new[] { "Місцезнаходження (місто)", "Місто", "City", "city", "Населений пункт" });
                int idxRegion = FindIndexFlexible(headers, new[] { "Місцезнаходження (область)", "Область", "Region", "region" });
                int idxAddress = FindIndexFlexible(headers, new[] { "Адреса", "Addreess", "Address", "address", "Місцезнаходження" });
                int idxEmail = FindIndexFlexible(headers, new[] { "Адреса електронної пошти", "Email", "email", "Пошта", "E-mail" });

                if (idxCode < 0)
                {
                    TempData["ErrorMessage"] = $"Не знайдено колонку з кодом (очікувались: Код, Code, ЄДРПОУ, РНОКПП). Знайдені колонки: {string.Join(" | ", headers)}";
                }

                // Get existing codes for fast lookup
                var existingCodes = await _db.ProcuringEntitys.Select(e => e.Code).ToListAsync();
                var existingSet = new HashSet<string>(existingCodes);

                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var fields = ParseCsvLine(line);
                    string code = idxCode >= 0 && idxCode < fields.Length ? fields[idxCode].Trim() : "";
                    if (string.IsNullOrEmpty(code) || existingSet.Contains(code))
                    {
                        skipped++;
                        continue;
                    }

                    string legalName = idxLegalName >= 0 && idxLegalName < fields.Length ? fields[idxLegalName].Trim() : "";
                    string name = idxName >= 0 && idxName < fields.Length ? fields[idxName].Trim() : "";
                    string city = idxCity >= 0 && idxCity < fields.Length ? fields[idxCity].Trim() : "";
                    string region = idxRegion >= 0 && idxRegion < fields.Length ? fields[idxRegion].Trim() : "";
                    string directAddress = idxAddress >= 0 && idxAddress < fields.Length ? fields[idxAddress].Trim() : "";

                    // Build address: prefer direct address, otherwise combine city + region
                    string address;
                    if (!string.IsNullOrEmpty(directAddress))
                        address = directAddress;
                    else
                        address = string.Join(", ", new[] { city, region }.Where(s => !string.IsNullOrEmpty(s)));

                    _db.ProcuringEntitys.Add(new ProcuringEntity
                    {
                        Code = code,
                        Name = name,
                        LegalName = legalName,
                        Addreess = address
                    });

                    existingSet.Add(code);
                    added++;
                }

                await _db.SaveChangesAsync();
                _logger.LogInformation("CSV import: added={Added}, skipped={Skipped}", added, skipped);
                TempData["SuccessMessage"] = $"Імпорт завершено: додано {added}, пропущено {skipped} (вже існують).";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CSV import error: {Message}", ex.Message);
                TempData["ErrorMessage"] = $"Помилка імпорту: {ex.Message}";
                if (ex.InnerException != null)
                    TempData["ErrorMessage"] += $" | Inner: {ex.InnerException.Message}";
            }
            return RedirectToPage();
        }

        private static int FindIndexFlexible(string[] headers, string[] possibleNames)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                var h = headers[i].Trim();
                foreach (var name in possibleNames)
                {
                    if (h.Equals(name, StringComparison.OrdinalIgnoreCase))
                        return i;
                }
            }
            // Partial match (contains) as fallback
            for (int i = 0; i < headers.Length; i++)
            {
                var h = headers[i].Trim();
                foreach (var name in possibleNames)
                {
                    if (h.Contains(name, StringComparison.OrdinalIgnoreCase))
                        return i;
                }
            }
            return -1;
        }

        private static string[] ParseCsvLine(string line)
        {
            var result = new List<string>();
            var sb = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        sb.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    result.Add(sb.ToString());
                    sb.Clear();
                }
                else
                {
                    sb.Append(c);
                }
            }
            result.Add(sb.ToString());
            return result.ToArray();
        }

        private async Task LoadEntitiesAsync()
        {
            CurrentPage = PageNumber ?? 1;
            if (CurrentPage < 1) CurrentPage = 1;

            var query = _db.ProcuringEntitys.AsQueryable();
            if (!string.IsNullOrWhiteSpace(SearchCode))
            {
                query = query.Where(e =>
                    e.Code.Contains(SearchCode) ||
                    (e.Name != null && e.Name.Contains(SearchCode)) ||
                    (e.LegalName != null && e.LegalName.Contains(SearchCode)));
            }

            TotalCount = await query.CountAsync();
            if (CurrentPage > TotalPages && TotalPages > 0) CurrentPage = TotalPages;

            Entities = await query
                .OrderBy(e => e.Code)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}
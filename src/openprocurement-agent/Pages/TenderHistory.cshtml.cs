using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using openprocurement_agent.Models;

namespace openprocurement_agent.Pages
{
    public class TenderHistoryModel : PageModel
    {
        private readonly TenderHistoryDbContext _db;

        public TenderHistoryModel(TenderHistoryDbContext db)
        {
            _db = db;
        }

        public List<TenderHistory> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 25;
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        [BindProperty(SupportsGet = true)]
        public string? SearchTenderId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? PageNumber { get; set; }

        public async Task OnGetAsync()
        {
            await LoadItemsAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string tenderId)
        {
            var entity = await _db.TenderHistory
                .FirstOrDefaultAsync(t => t.TenderId == tenderId);
            if (entity == null)
            {
                TempData["ErrorMessage"] = "Запис не знайдено.";
                return RedirectToPage();
            }

            _db.TenderHistory.Remove(entity);
            await _db.SaveChangesAsync();
            TempData["SuccessMessage"] = "Запис видалено.";
            return RedirectToPage(new { SearchTenderId, PageNumber = CurrentPage });
        }

        private async Task LoadItemsAsync()
        {
            CurrentPage = PageNumber ?? 1;
            if (CurrentPage < 1) CurrentPage = 1;

            var query = _db.TenderHistory.AsQueryable();
            if (!string.IsNullOrWhiteSpace(SearchTenderId))
            {
                query = query.Where(t => t.TenderId.Contains(SearchTenderId));
            }

            TotalCount = await query.CountAsync();
            if (CurrentPage > TotalPages && TotalPages > 0) CurrentPage = TotalPages;

            Items = await query
                .OrderByDescending(t => t.CreatedDate)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}
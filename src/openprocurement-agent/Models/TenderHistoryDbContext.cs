using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace openprocurement_agent.Models
{
    public class TenderHistoryDbContext : DbContext
    {
        public DbSet<TenderHistory> TenderHistory { get; set; }

        public TenderHistoryDbContext(DbContextOptions<TenderHistoryDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
            Database.Migrate();
        }

        public TenderHistoryDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlite(@"Data Source=TenderHistory.db");
            }

            // Suppress the pending model changes warning (optional)
            options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }
    }


    public class TenderHistory
    {

        [Key]
        public required string TenderId { get; set; }

        public string? Title { get; set; }

        public DateTime CreatedDate { get; set; }

        public TenderHistory()
        {
            this.CreatedDate = DateTime.UtcNow;
        }
    }

}

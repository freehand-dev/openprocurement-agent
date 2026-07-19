using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace openprocurement_agent.Models
{
    public class ProcuringEntityDbContext : DbContext
    {
        public DbSet<ProcuringEntity> ProcuringEntitys { get; set; }

        public ProcuringEntityDbContext(DbContextOptions<ProcuringEntityDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
            Database.Migrate();
        }

        public ProcuringEntityDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlite(@"Data Source=ProcuringEntity.db");
            }

            // Suppress the pending model changes warning (optional)
            options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }
    }

    public class ProcuringEntity
    {
        [Key]
        public required string Code { get; set; }
        public string? Name { get; set; }
        public string? LegalName { get; set; }
        public string? Addreess { get; set; }
    }
}

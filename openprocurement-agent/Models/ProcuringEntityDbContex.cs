using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace openprocurement_agent.Models
{
    public class ProcuringEntityDbContex : DbContext
    {
        public DbSet<ProcuringEntity> ProcuringEntitys { get; set; }


        public ProcuringEntityDbContex(DbContextOptions<ProcuringEntityDbContex> options)
            : base(options) 
        {
            Database.EnsureCreated();
            Database.Migrate();
        }

        public ProcuringEntityDbContex()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlite(@"Data Source=ProcuringEntity.db");
            }
        }
    }

    public class ProcuringEntity
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string LegalName { get; set; }
        public string Addreess { get; set; }

        public string Telephone { get; set; }
    }
}

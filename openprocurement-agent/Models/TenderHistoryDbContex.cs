using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace openprocurement_agent.Models
{
    public class TenderHistoryDbContex : DbContext
    {
        public DbSet<TenderHistory> TenderHistory { get; set; }

        public TenderHistoryDbContex(DbContextOptions<TenderHistoryDbContex> options)
            : base(options) 
        {
            Database.EnsureCreated();
            Database.Migrate();
        }

        public TenderHistoryDbContex()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlite(@"Data Source=TenderHistory.db");
            }
        }
    }


    public class TenderHistory
    {

        [Key]
        public string TenderId { get; set; }

        public DateTime CreatedDate { get; set; }

        public TenderHistory()
        {
            this.CreatedDate = DateTime.UtcNow;
        }
    }

}

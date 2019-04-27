using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Annex.Models
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext() : base("name=CustomerDbContext")
        {
        }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasIndex(e => e.NationalId)
                .IsUnique();

            modelBuilder.Entity<Customer>()
               .HasIndex(e => e.MobileNo)
               .IsUnique();

        }
    }
}
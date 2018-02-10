using System;
using hubMarket_domain;
using Microsoft.EntityFrameworkCore;

namespace hubMarket_data
{
    public class Context: DbContext
    {
        public Context()
        {
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=localhost;Initial Catalog=hubMarket;User ID=sa;Password=SqlExpress123;");
        }

        public DbSet<User> Users { get; set; }
    }
}
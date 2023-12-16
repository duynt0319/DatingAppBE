using API.Entities;
using API.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<AppUser>().HasData(
        //        new AppUser { Id = 1, UserName = "John" },
        //        new AppUser { Id = 2, UserName = "Duy" }
        //    );
        //}
        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {

            builder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverterExtensions>()
                .HaveColumnType("date");

            base.ConfigureConventions(builder);

        }
    }
}

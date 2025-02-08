// AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using VitalityBuilder.Api.Models;

namespace VitalityBuilder.Api.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Character> Characters { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure your model here
        }
    }
}
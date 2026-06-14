using Microsoft.EntityFrameworkCore;
using WorkLogPro.Models;

namespace WorkLogPro.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<WorkItem> WorkItems { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;

    // Migration Seeding
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     base.OnModelCreating(modelBuilder);

    //     modelBuilder.Entity<Category>().HasData(
    //         new Category { Id = 1, Name = "Reporting" },
    //         new Category { Id = 2, Name = "Carrier Follow-Up" },
    //         new Category { Id = 3, Name = "Admin" },
    //         new Category { Id = 4, Name = "Yard" },
    //         new Category { Id = 5, Name = "Equipment" },
    //         new Category { Id = 6, Name = "Other" }
    //     );
    // }
}
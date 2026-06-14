using WorkLogPro.Models;

namespace WorkLogPro.Data;

// Runtime Seeding
public static class DbInitializer
{
    public static void Seed(AppDbContext context)
    {
        if (context.Categories.Any())
        {
            return;
        }
        
        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category { Name = "Reporting" },
                new Category { Name = "Carrier Follow-Up" },
                new Category { Name = "Admin" },
                new Category { Name = "Yard" },
                new Category { Name = "Equipment" },
                new Category { Name = "Other" }
            );
        }

        context.SaveChanges();
    }
}
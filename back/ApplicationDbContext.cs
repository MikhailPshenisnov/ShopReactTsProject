using Microsoft.EntityFrameworkCore;

namespace back;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasKey(h => new { h.id, h.username, h.password, h.cart, h.end_date });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=web_project_users;" +
                                 "Username=postgres;Password=admin");
    }
}
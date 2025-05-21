using Microsoft.EntityFrameworkCore;
using PatientRecovery.NotificationService.Models;

namespace PatientRecovery.NotificationService.Data
{
    public class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Notification> Notifications { get; set; }
    public DbSet<NotificationTemplate> NotificationTemplates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Notification>()
            .Property(n => n.Type)
            .HasConversion<string>();

        modelBuilder.Entity<Notification>()
            .Property(n => n.Priority)
            .HasConversion<string>();

        modelBuilder.Entity<Notification>()
            .Property(n => n.Status)
            .HasConversion<string>();

        modelBuilder.Entity<NotificationTemplate>()
            .Property(t => t.Type)
            .HasConversion<string>();
    }
}
}
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

        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SenderId).IsRequired();
                entity.Property(e => e.ReceiverId).IsRequired();
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.Timestamp).IsRequired();
                entity.Property(e => e.SenderRole).IsRequired();
            });
        }
    }
}
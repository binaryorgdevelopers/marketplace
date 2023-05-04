using Microsoft.EntityFrameworkCore;
using NotificationService.Persistence.Entities;

namespace NotificationService.DAL;

public class NotificationContext : DbContext
{
    public NotificationContext(DbContextOptions<NotificationContext> options) : base(options)
    {
    }

    public DbSet<Notification> Notifications { get; set; }
}
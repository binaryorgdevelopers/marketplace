﻿using Microsoft.EntityFrameworkCore;
using NotificationService.Persistence.Entities;

namespace NotificationService.Persistence;

public class NotificationContext : DbContext
{
    public NotificationContext(DbContextOptions<NotificationContext> options) : base(options)
    {
    }

    public DbSet<Notification> Notifications { get; set; }
}
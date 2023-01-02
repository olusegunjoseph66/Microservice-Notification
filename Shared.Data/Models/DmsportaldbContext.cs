using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Shared.Data.Models
{
    public partial class DmsportaldbContext : DbContext
    {
        public DmsportaldbContext()
        {
        }

        public DmsportaldbContext(DbContextOptions<DmsportaldbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<Unsubscribe> Unsubscribes { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        public virtual DbSet<UserNotification> UserNotifications { get; set; } = null!;

        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        //                optionsBuilder.UseSqlServer("Server=.;Initial Catalog=dms-portal-db; Trusted_Connection=false;User=sa;Password=Enochadeboyes_9");
        //            }
        //        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notifications", "Notifications");

                entity.Property(e => e.EmailTemplateId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EventTriggerName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PushMessageTemplate)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SmsMessageTemplate)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Unsubscribe>(entity =>
            {
                entity.ToTable("Unsubscribes", "Notifications");

                entity.Property(e => e.DateUnsubscribed).HasColumnType("datetime");

                entity.HasOne(d => d.Notification)
                    .WithMany(p => p.Unsubscribes)
                    .HasForeignKey(d => d.NotificationId)
                    .HasConstraintName("FK_Unsubscribes_Notifications");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Unsubscribes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Unsubscribes_Users");
            });

            modelBuilder.Entity<UserNotification>(entity =>
            {
                entity.ToTable("UserNotifications", "Notifications");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateRead).HasColumnType("datetime");

               

                
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users", "Notifications");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.DeviceId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Roles).HasMaxLength(500);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

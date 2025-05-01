using System;
using System.Collections.Generic;
using FitnessCenterApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterApi.Data;

public partial class FitnessCenterDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    

    public FitnessCenterDbContext(DbContextOptions<FitnessCenterDbContext> options) : base(options)
    {
        
    }

    public virtual DbSet<FitnessCentar> FitnessCentars { get; set; }

    public virtual DbSet<Membership> Memberships { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<ShopItem> ShopItems { get; set; }
    public DbSet<UserItems> UserItems { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public DbSet<Attendance> Attendances { get; set; }

    
    public DbSet<Article> Articles { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<EventParticipant> EventParticipants { get; set; }
    public DbSet<Coach> Coaches { get; set; }
    public DbSet<CoachProgram> CoachPrograms { get; set; }
    public DbSet<FitnessCenterUser> FitnessCenterUsers { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=fitnessCenterDB.sqlite");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Article
        modelBuilder.Entity<Article>()
            .HasOne(a => a.FitnessCentar)
            .WithMany()
            .HasForeignKey(a => a.IdFitnessCentar);

// Event
        modelBuilder.Entity<Event>()
            .HasOne(e => e.FitnessCentar)
            .WithMany()
            .HasForeignKey(e => e.IdFitnessCentar);

// EventParticipant (join)
        modelBuilder.Entity<EventParticipant>()
            .HasKey(ep => new { ep.IdEvent, ep.IdUser });

        modelBuilder.Entity<EventParticipant>()
            .HasOne(ep => ep.Event)
            .WithMany(e => e.Participants)
            .HasForeignKey(ep => ep.IdEvent);

        modelBuilder.Entity<EventParticipant>()
            .HasOne(ep => ep.User)
            .WithMany()
            .HasForeignKey(ep => ep.IdUser);

// Coach
        modelBuilder.Entity<Coach>()
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.IdUser);

// CoachProgram
        modelBuilder.Entity<CoachProgram>()
            .HasOne(cp => cp.Coach)
            .WithMany()
            .HasForeignKey(cp => cp.IdCoach);

        modelBuilder.Entity<CoachProgram>()
            .HasOne(cp => cp.FitnessCentar)
            .WithMany()
            .HasForeignKey(cp => cp.IdFitnessCentar);

// FitnessCenterUser (join)
        modelBuilder.Entity<FitnessCenterUser>()
            .HasKey(fu => new { fu.IdUser, fu.IdFitnessCentar });

        modelBuilder.Entity<FitnessCenterUser>()
            .HasOne(fu => fu.User)
            .WithMany()
            .HasForeignKey(fu => fu.IdUser);

        modelBuilder.Entity<FitnessCenterUser>()
            .HasOne(fu => fu.FitnessCentar)
            .WithMany()
            .HasForeignKey(fu => fu.IdFitnessCentar);
        
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany()
            .HasForeignKey(m => m.IdSender)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Recipient)
            .WithMany()
            .HasForeignKey(m => m.IdRecipient)
            .OnDelete(DeleteBehavior.Restrict);

// Notification
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany()
            .HasForeignKey(n => n.IdUser)
            .OnDelete(DeleteBehavior.Cascade);

// ShopItem
        modelBuilder.Entity<ShopItem>()
            .HasOne(si => si.FitnessCentar)
            .WithMany()
            .HasForeignKey(si => si.IdFitnessCentar)
            .OnDelete(DeleteBehavior.Cascade);
        
        //attendance
        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.IdAttendance);

            entity.HasOne(a => a.User)
                .WithMany() // or .WithMany(u => u.Attendances) if you add a collection in `User`
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(a => a.FitnessCentar)
                .WithMany() // or .WithMany(f => f.Attendances) if you add a collection in `FitnessCentar`
                .HasForeignKey(a => a.FitnessCentarId)
                .OnDelete(DeleteBehavior.Cascade);
        });

// UserItem (many-to-many)
        modelBuilder.Entity<UserItems>()
            .HasKey(ui => new { ui.IdUser, ui.IdShopItem });

        modelBuilder.Entity<UserItems>()
            .HasOne(ui => ui.User)
            .WithMany()
            .HasForeignKey(ui => ui.IdUser);

        modelBuilder.Entity<UserItems>()
            .HasOne(ui => ui.ShopItem)
            .WithMany()
            .HasForeignKey(ui => ui.IdShopItem);

        modelBuilder.Entity<FitnessCentar>(entity =>
        {
            entity.HasKey(e => e.IdFitnessCentar);

            entity.ToTable("Fitness_Centar");

            entity.HasIndex(e => e.Name, "IX_Fitness_Centar_name").IsUnique();

            entity.Property(e => e.IdFitnessCentar).HasColumnName("id_fitness_centar");
            entity.Property(e => e.Name)
                .HasColumnType("VARCHAR(255)")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Membership>(entity =>
        {
            
            entity.HasKey(e => e.IdMembership);

            entity.ToTable("Membership");

            entity.Property(e => e.IdMembership).HasColumnName("id_membership");
            entity.Property(e => e.IdFitnessCentar).HasColumnName("id_fitness_centar");
            entity.Property(e => e.LoyaltyPoints)
                .HasDefaultValue(0)
                .HasColumnName("loyalty_points");
            entity.Property(e => e.MembershipCountdown)
                .HasDefaultValue(0)
                .HasColumnName("membershipCountdown");
            entity.Property(e => e.StreakRunCount)
                .HasDefaultValue(0)
                .HasColumnName("streak_run_count");

            entity.HasOne(d => d.IdFitnessCentarNavigation).WithMany(p => p.Memberships)
                .HasForeignKey(d => d.IdFitnessCentar)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Memberships)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "IX_User_email").IsUnique();

            entity.HasIndex(e => e.PictureLink, "IX_User_picture_link").IsUnique();

            entity.HasIndex(e => e.SecretKey, "IX_User_secret_key").IsUnique();

            entity.HasIndex(e => e.UserName, "IX_User_username").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id_user");
            entity.Property(e => e.Birthday)
                .HasColumnType("DATE")
                .HasColumnName("birthday");
            entity.Property(e => e.CreationDate)
                .HasColumnType("DATE")
                .HasColumnName("creation_date");
            entity.Property(e => e.Email)
                .HasColumnType("VARCHAR(255)")
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasColumnType("VARCHAR(255)")
                .HasColumnName("first_name");
            entity.Property(e => e.IsStudent).HasColumnName("is_student");
            entity.Property(e => e.LastName)
                .HasColumnType("VARCHAR(255)")
                .HasColumnName("last_name");
            entity.Property(e => e.Phone)
                .HasColumnType("VARCHAR(255)")
                .HasColumnName("phone");
            entity.Property(e => e.PictureLink)
                .HasColumnType("VARCHAR(2048)")
                .HasColumnName("picture_link");
            entity.Property(e => e.SecretKey)
                .HasColumnType("VARCHAR(255)")
                .HasColumnName("secret_key");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UserName)
                .HasColumnType("VARCHAR(255)")
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

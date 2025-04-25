using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterApi.Models;

public partial class FitnessCenterDbContext : DbContext
{
    public FitnessCenterDbContext()
    {
    }

    public FitnessCenterDbContext(DbContextOptions<FitnessCenterDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FitnessCentar> FitnessCentars { get; set; }

    public virtual DbSet<Membership> Memberships { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=fitnessCenterDB.sqlite");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
            entity.Property(e => e.IdUser).HasColumnName("id_user");
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
            entity.HasKey(e => e.IdUser);

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "IX_User_email").IsUnique();

            entity.HasIndex(e => e.PictureLink, "IX_User_picture_link").IsUnique();

            entity.HasIndex(e => e.SecretKey, "IX_User_secret_key").IsUnique();

            entity.HasIndex(e => e.Username, "IX_User_username").IsUnique();

            entity.Property(e => e.IdUser).HasColumnName("id_user");
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
            entity.Property(e => e.Username)
                .HasColumnType("VARCHAR(255)")
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

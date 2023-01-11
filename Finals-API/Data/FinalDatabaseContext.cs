using System;
using System.Collections.Generic;
using Finals_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Finals_API.Data;

public partial class FinalDatabaseContext : DbContext
{
    public FinalDatabaseContext(DbContextOptions<FinalDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Profile> Profiles { get; set; }
    public virtual DbSet<Quiz> Quizzes { get; set; }
    public virtual DbSet<Score> Scores { get; set; }
    public virtual DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_Profiles_1");

            entity.Property(e => e.UserName).HasMaxLength(50);

            entity.HasOne(d => d.User).WithOne(p => p.Profile)
                .HasForeignKey<Profile>(d => d.UserId)
                .HasConstraintName("FK_Profiles_Users1");
        });

        modelBuilder.Entity<Quiz>(entity =>
        {
            entity.ToTable("Quiz");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Date).HasColumnType("datetime");
        });

        modelBuilder.Entity<Score>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_Scores_1");

            entity.Property(e => e.LastScored).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithOne(p => p.Score)
                .HasForeignKey<Score>(d => d.UserId)
                .HasConstraintName("FK_Scores_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Users_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

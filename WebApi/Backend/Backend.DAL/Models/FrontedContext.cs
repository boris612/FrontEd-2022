﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Backend.DAL.Models
{
  public partial class FrontedContext : DbContext
    {
        public FrontedContext(DbContextOptions<FrontedContext> options)
            : base(options)
        {
        }

        public virtual DbSet<RefreshToken> RefreshToken { get; set; }
        public virtual DbSet<School> School { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<Town> Town { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<ViewWorkshops> ViewWorkshops { get; set; }
        public virtual DbSet<Workshop> Workshop { get; set; }
        public virtual DbSet<WorkshopParticipant> WorkshopParticipant { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Croatian_CI_AS");

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RefreshToken)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_RefreshToken_User");
            });

            modelBuilder.Entity<School>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.TownId }, "IX_School")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Town)
                    .WithMany(p => p.School)
                    .HasForeignKey(d => d.TownId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_School_Town");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasIndex(e => e.Email, "UIX_Student_Email")
                    .IsUnique();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.School)
                    .WithMany(p => p.Student)
                    .HasForeignKey(d => d.SchoolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Student_School");
            });

            modelBuilder.Entity<Town>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.FirstName).HasMaxLength(200);

                entity.Property(e => e.LastName).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Password).HasMaxLength(200);
            });

            modelBuilder.Entity<ViewWorkshops>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("view_Workshops");

                entity.Property(e => e.School)
                    .IsRequired()
                    .HasMaxLength(301);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Workshop>(entity =>
            {
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.School)
                    .WithMany(p => p.Workshop)
                    .HasForeignKey(d => d.SchoolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Workshop_School");
            });

            modelBuilder.Entity<WorkshopParticipant>(entity =>
            {
                entity.HasIndex(e => new { e.ParticipantId, e.WorkshopId }, "IX_WorkshopParticipant")
                    .IsUnique();

                entity.HasOne(d => d.Participant)
                    .WithMany(p => p.WorkshopParticipant)
                    .HasForeignKey(d => d.ParticipantId)
                    .HasConstraintName("FK_WorkshopParticipant_Student");

                entity.HasOne(d => d.Workshop)
                    .WithMany(p => p.WorkshopParticipant)
                    .HasForeignKey(d => d.WorkshopId)
                    .HasConstraintName("FK_WorkshopParticipant_Workshop");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
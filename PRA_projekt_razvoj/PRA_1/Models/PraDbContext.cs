using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PRA_1.Models;

public partial class PraDbContext : DbContext
{
    public PraDbContext()
    {
    }

    public PraDbContext(DbContextOptions<PraDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=ConnectionStrings:ConnStringPRA");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Iduser).HasName("PK__User__EAE6D9DF61B140BC");

            entity.ToTable("User");

            entity.HasIndex(e => e.Phone, "UQ__User__5C7E359E247591EC").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__User__A9D10534317E36AD").IsUnique();

            entity.HasIndex(e => e.UserName, "UQ__User__C9F28456830FE2B4").IsUnique();

            entity.Property(e => e.Iduser).HasColumnName("IDUser");
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.FirstName).HasMaxLength(256);
            entity.Property(e => e.LastName).HasMaxLength(256);
            entity.Property(e => e.Phone).HasMaxLength(256);
            entity.Property(e => e.TestPassword).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);
            entity.Property(e => e.UserPassword).HasMaxLength(256);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

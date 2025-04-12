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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=FRANJO3003;Database=PRA_db;User=sa;Password=SQL;TrustServerCertificate=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Iduser).HasName("PK__User__EAE6D9DF084DFCB3");

            entity.ToTable("User");

            entity.HasIndex(e => e.Phone, "UQ__User__5C7E359E19334E45").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__User__A9D10534E46DB62B").IsUnique();

            entity.HasIndex(e => e.UserName, "UQ__User__C9F28456BE9EEC60").IsUnique();

            entity.Property(e => e.Iduser).HasColumnName("IDUser");
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.FirstName).HasMaxLength(256);
            entity.Property(e => e.LastName).HasMaxLength(256);
            entity.Property(e => e.Phone).HasMaxLength(256);
            entity.Property(e => e.Temp2Facode)
                .HasMaxLength(256)
                .HasColumnName("Temp2FACode");
            entity.Property(e => e.Temp2FacodeExpires)
                .HasColumnType("datetime")
                .HasColumnName("Temp2FACodeExpires");
            entity.Property(e => e.TestPassword).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);
            entity.Property(e => e.UserPassword).HasMaxLength(256);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

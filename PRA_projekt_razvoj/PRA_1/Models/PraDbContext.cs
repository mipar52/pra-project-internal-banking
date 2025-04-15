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

    public virtual DbSet<CarRegistration> CarRegistrations { get; set; }

    public virtual DbSet<CreditCard> CreditCards { get; set; }

    public virtual DbSet<CreditCardDataBase> CreditCardDatabases { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserCarRegistration> UserCarRegistrations { get; set; }

    public virtual DbSet<UserCreditCard> UserCreditCards { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=FRANJO3003;Database=PRA_db;User=sa;Password=SQL;TrustServerCertificate=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CarRegistration>(entity =>
        {
            entity.HasKey(e => e.IdcarRegistration).HasName("PK__CarRegis__9498895E6873B755");

            entity.ToTable("CarRegistration");

            entity.HasIndex(e => new { e.RegistrationCountry, e.RegistrationNumber }, "UQ_RegistrationCountry_Number").IsUnique();

            entity.Property(e => e.IdcarRegistration).HasColumnName("IDCarRegistration");
            entity.Property(e => e.CarBrand).HasMaxLength(256);
            entity.Property(e => e.CarModel).HasMaxLength(256);
            entity.Property(e => e.RegistrationCountry).HasMaxLength(256);
            entity.Property(e => e.RegistrationNumber).HasMaxLength(256);
        });

        modelBuilder.Entity<CreditCard>(entity =>
        {
            entity.HasKey(e => e.IdcreditCard).HasName("PK__CreditCa__21F545B157896DF1");

            entity.ToTable("CreditCard");

            entity.HasIndex(e => e.CardNumber, "UQ__CreditCa__A4E9FFE9B48E0974").IsUnique();

            entity.Property(e => e.IdcreditCard).HasColumnName("IDCreditCard");
            entity.Property(e => e.CardNumber).HasMaxLength(16);
            entity.Property(e => e.CvvHash).HasMaxLength(256);
            entity.Property(e => e.CvvSalt).HasMaxLength(256);
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.FirstName).HasMaxLength(256);
            entity.Property(e => e.LastName).HasMaxLength(256);
        });

        modelBuilder.Entity<CreditCardDataBase>(entity =>
        {
            entity.HasKey(e => e.IdcreditCardDataBase).HasName("PK__CreditCa__61862752FB181AAB");

            entity.ToTable("CreditCardDataBase");

            entity.HasIndex(e => e.CardNumber, "UQ__CreditCa__A4E9FFE9158DFDAA").IsUnique();

            entity.Property(e => e.IdcreditCardDataBase).HasColumnName("IDCreditCardDataBase");
            entity.Property(e => e.CardNumber).HasMaxLength(16);
            entity.Property(e => e.CvvHash).HasMaxLength(256);
            entity.Property(e => e.CvvSalt).HasMaxLength(256);
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.FirstName).HasMaxLength(256);
            entity.Property(e => e.LastName).HasMaxLength(256);
        });

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

        modelBuilder.Entity<UserCarRegistration>(entity =>
        {
            entity.HasKey(e => e.IduserCarRegistration).HasName("PK__UserCarR__0A388D0349685C74");

            entity.ToTable("UserCarRegistration");

            entity.HasIndex(e => new { e.UserId, e.CarRegistrationId }, "UQ_User_CarRegistration").IsUnique();

            entity.Property(e => e.IduserCarRegistration).HasColumnName("IDUserCarRegistration");
            entity.Property(e => e.CarRegistrationId).HasColumnName("CarRegistrationID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.CarRegistration).WithMany(p => p.UserCarRegistrations)
                .HasForeignKey(d => d.CarRegistrationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserCarRe__CarRe__282DF8C2");

            entity.HasOne(d => d.User).WithMany(p => p.UserCarRegistrations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserCarRe__UserI__2739D489");
        });

        modelBuilder.Entity<UserCreditCard>(entity =>
        {
            entity.HasKey(e => e.IduserCreditCard).HasName("PK__UserCred__9C51B4F656456F70");

            entity.ToTable("UserCreditCard");

            entity.Property(e => e.IduserCreditCard).HasColumnName("IDUserCreditCard");
            entity.Property(e => e.CreditCardId).HasColumnName("CreditCardID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.CreditCard).WithMany(p => p.UserCreditCards)
                .HasForeignKey(d => d.CreditCardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserCredi__Credi__7B5B524B");

            entity.HasOne(d => d.User).WithMany(p => p.UserCreditCards)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserCredi__UserI__7A672E12");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

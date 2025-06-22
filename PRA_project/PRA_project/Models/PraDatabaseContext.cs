using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PRA_project.Models;

public partial class PraDatabaseContext : DbContext
{
    public PraDatabaseContext()
    {
    }

    public PraDatabaseContext(DbContextOptions<PraDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BillingAccount> BillingAccounts { get; set; }

    public virtual DbSet<CreditCard> CreditCards { get; set; }

    public virtual DbSet<Friend> Friends { get; set; }

    public virtual DbSet<MoneyTransfer> MoneyTransfers { get; set; }

    public virtual DbSet<ParkingPayment> ParkingPayments { get; set; }

    public virtual DbSet<RequestTransfer> RequestTransfers { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<StudyProgram> StudyPrograms { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TransactionType> TransactionTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserCreditCard> UserCreditCards { get; set; }


    // !! Odkomentirati !! ovo je za potrebe testove zakomentirano
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("server=localhost;Database=PRA_database;User=sa;Password=SQL;TrustServerCertificate=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BillingAccount>(entity =>
        {
            entity.HasKey(e => e.IdBillingAccount).HasName("PK__BillingA__1F29EB5A9C0BC4ED");

            entity.ToTable("BillingAccount");

            entity.Property(e => e.Balance).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.User).WithMany(p => p.BillingAccounts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__BillingAc__UserI__72C60C4A");
        });

        modelBuilder.Entity<CreditCard>(entity =>
        {
            entity.HasKey(e => e.IdCreditCard).HasName("PK__CreditCa__7F489E9257732C4E");

            entity.ToTable("CreditCard");

            entity.Property(e => e.CreditCardNumber).HasMaxLength(16);
            entity.Property(e => e.Cvvhash)
                .HasMaxLength(256)
                .HasColumnName("CVVhash");
            entity.Property(e => e.Cvvsalt)
                .HasMaxLength(256)
                .HasColumnName("CVVsalt");
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.FirstName).HasMaxLength(256);
            entity.Property(e => e.Lastname).HasMaxLength(256);
        });

        modelBuilder.Entity<Friend>(entity =>
        {
            entity.HasKey(e => e.IdFriend).HasName("PK__Friend__30F0D952780EAA0F");

            entity.ToTable("Friend");

            entity.HasOne(d => d.FriendNavigation).WithMany(p => p.FriendFriendNavigations)
                .HasForeignKey(d => d.FriendId)
                .HasConstraintName("FK__Friend__FriendId__6FE99F9F");

            entity.HasOne(d => d.User).WithMany(p => p.FriendUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Friend__UserId__6EF57B66");
        });

        modelBuilder.Entity<MoneyTransfer>(entity =>
        {
            entity.HasKey(e => e.IdMoneyTransfer).HasName("PK__MoneyTra__BA7A5547D74B700F");

            entity.ToTable("MoneyTransfer");

            entity.HasOne(d => d.Transaction).WithMany(p => p.MoneyTransfers)
                .HasForeignKey(d => d.TransactionId)
                .HasConstraintName("FK__MoneyTran__Trans__6B24EA82");

            entity.HasOne(d => d.UserReciever).WithMany(p => p.MoneyTransfers)
                .HasForeignKey(d => d.UserRecieverId)
                .HasConstraintName("FK__MoneyTran__UserR__6C190EBB");
        });

        modelBuilder.Entity<ParkingPayment>(entity =>
        {
            entity.HasKey(e => e.IdParkingPayment).HasName("PK__ParkingP__43AAD255EA483CFB");

            entity.ToTable("ParkingPayment");

            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.RegistrationCountryCode).HasMaxLength(256);
            entity.Property(e => e.RegistrationNumber).HasMaxLength(256);
            entity.Property(e => e.StartTime).HasColumnType("datetime");

            entity.HasOne(d => d.Transaction).WithMany(p => p.ParkingPayments)
                .HasForeignKey(d => d.TransactionId)
                .HasConstraintName("FK__ParkingPa__Trans__68487DD7");
        });

        modelBuilder.Entity<RequestTransfer>(entity =>
        {
            entity.HasKey(e => e.IdRequestTransfer).HasName("PK__RequestT__7B471779EF48FAA1");

            entity.ToTable("RequestTransfer");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.HasOne(d => d.UserReciever).WithMany(p => p.RequestTransferUserRecievers)
                .HasForeignKey(d => d.UserRecieverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RequestTr__UserR__02FC7413");

            entity.HasOne(d => d.UserSender).WithMany(p => p.RequestTransferUserSenders)
                .HasForeignKey(d => d.UserSenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RequestTr__UserS__03F0984C");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PK__Role__B43690543B41BDCC");

            entity.ToTable("Role");

            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<StudyProgram>(entity =>
        {
            entity.HasKey(e => e.IdStudyProgram).HasName("PK__StudyPro__BF4E63D51FD9FD62");

            entity.ToTable("StudyProgram");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.IdTransaction).HasName("PK__Transact__45542F458945AD40");

            entity.ToTable("Transaction");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.HasOne(d => d.TransactionType).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.TransactionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Trans__656C112C");

            entity.HasOne(d => d.User).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__UserI__6477ECF3");
        });

        modelBuilder.Entity<TransactionType>(entity =>
        {
            entity.HasKey(e => e.IdTransactionType).HasName("PK__Transact__CC0038D94AB00B35");

            entity.ToTable("TransactionType");

            entity.Property(e => e.TypeName).HasMaxLength(256);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__user__B7C926382F2CDE0F");

            entity.ToTable("User");

            entity.Property(e => e.EmailAddress).HasMaxLength(256);
            entity.Property(e => e.FirstName).HasMaxLength(256);
            entity.Property(e => e.LastName).HasMaxLength(256);
            entity.Property(e => e.PasswordHash).HasMaxLength(256);
            entity.Property(e => e.PasswordSalt).HasMaxLength(256);
            entity.Property(e => e.PhoneNumber).HasMaxLength(256);
            entity.Property(e => e.ProfilePictureUrl).HasMaxLength(256);
            entity.Property(e => e.Temp2Facode)
                .HasMaxLength(256)
                .HasColumnName("Temp2FACode");
            entity.Property(e => e.Temp2FacodeExpires)
                .HasColumnType("datetime")
                .HasColumnName("Temp2FACodeExpires");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__user__RoleId__5629CD9C");

            entity.HasOne(d => d.StudyProgram).WithMany(p => p.Users)
                .HasForeignKey(d => d.StudyProgramId)
                .HasConstraintName("FK__user__StudyProgr__571DF1D5");
        });

        modelBuilder.Entity<UserCreditCard>(entity =>
        {
            entity.HasKey(e => e.IdUserCreditCard).HasName("PK__UserCred__D4E13B996919C50B");

            entity.ToTable("UserCreditCard");

            entity.HasOne(d => d.CreditCard).WithMany(p => p.UserCreditCards)
                .HasForeignKey(d => d.CreditCardId)
                .HasConstraintName("FK__UserCredi__Credi__5FB337D6");

            entity.HasOne(d => d.User).WithMany(p => p.UserCreditCards)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserCredi__UserI__5EBF139D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

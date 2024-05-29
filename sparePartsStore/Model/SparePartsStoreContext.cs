using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace sparePartsStore.Model;

public partial class SparePartsStoreContext : DbContext
{
    public SparePartsStoreContext()
    {
    }

    public SparePartsStoreContext(DbContextOptions<SparePartsStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Autopart> Autoparts { get; set; }

    public virtual DbSet<CarBrand> CarBrands { get; set; }

    public virtual DbSet<CarModel> CarModels { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Knot> Knots { get; set; }

    public virtual DbSet<Manufacture> Manufactures { get; set; }

    public virtual DbSet<PartInterchangeability> PartInterchangeabilities { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ALEX_BEREZKIN\\SQLEXPRESS;Database=sparePartsStore;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=True; encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__account__F267251E1E5430B0");

            entity.ToTable("account");

            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.AccountLogin)
                .HasMaxLength(300)
                .HasColumnName("accountLogin");
            entity.Property(e => e.AccountPassword)
                .HasMaxLength(150)
                .HasColumnName("accountPassword");
            entity.Property(e => e.AccountRoleName).HasMaxLength(50).HasColumnName("accountRoleName");
            entity.Property(e => e.Inn)
                .HasMaxLength(12)
                .HasColumnName("INN");
            entity.Property(e => e.Kpp)
                .HasMaxLength(9)
                .HasColumnName("KPP");
            entity.Property(e => e.NameOrganization).HasMaxLength(50).HasColumnName("nameOrganization");
            entity.Property(e => e.Ogrn)
                .HasMaxLength(13)
                .HasColumnName("OGRN");
            entity.Property(e => e.Ogrnip)
                .HasMaxLength(15)
                .HasColumnName("OGRNIP");
        });


        modelBuilder.Entity<Autopart>(entity =>
        {
            entity.HasKey(e => e.AutopartId).HasName("PK__autopart__1FE2CE15C5745E68");

            entity.ToTable("autopart");

            entity.Property(e => e.AutopartId).HasColumnName("autopartId");
            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.AvailableityStock).HasColumnName("availableityStock");
            entity.Property(e => e.CarModelId).HasColumnName("carModelId");
            entity.Property(e => e.KnotId).HasColumnName("knotId");
            entity.Property(e => e.ManufactureId).HasColumnName("manufactureId");
            entity.Property(e => e.ModerationStatus)
                .HasMaxLength(150)
                .HasColumnName("moderationStatus");
            entity.Property(e => e.NameAutopart)
                .HasMaxLength(150)
                .HasColumnName("nameAutopart");
            entity.Property(e => e.NumberAutopart).HasColumnName("numberAutopart");
            entity.Property(e => e.PriceSale)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("priceSale");

            entity.HasOne(d => d.Account).WithMany(p => p.Autoparts)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("fk_accountId_account");

            entity.HasOne(d => d.CarModel).WithMany(p => p.Autoparts)
                .HasForeignKey(d => d.CarModelId)
                .HasConstraintName("fk_carModelId_carModel");

            entity.HasOne(d => d.Knot).WithMany(p => p.Autoparts)
                .HasForeignKey(d => d.KnotId)
                .HasConstraintName("fk_knotId_knot");

            entity.HasOne(d => d.Manufacture).WithMany(p => p.Autoparts)
                .HasForeignKey(d => d.ManufactureId)
                .HasConstraintName("fk_manufactureId_manufacture");
        });

        modelBuilder.Entity<CarBrand>(entity =>
        {
            entity.HasKey(e => e.CarBrandId).HasName("PK__carBrand__6E0F4E8F07CC5A09");

            entity.ToTable("carBrand");

            entity.Property(e => e.CarBrandId).HasColumnName("carBrandId");
            entity.Property(e => e.NameCarBrand)
                .HasMaxLength(150)
                .HasColumnName("nameCarBrand");
        });

        modelBuilder.Entity<CarModel>(entity =>
        {
            entity.HasKey(e => e.CarModelId).HasName("PK__carModel__0A869DCEA8DCC33C");

            entity.ToTable("carModel");

            entity.Property(e => e.CarModelId).HasColumnName("carModelId");
            entity.Property(e => e.CarBrandId).HasColumnName("carBrandId");
            entity.Property(e => e.NameCarModel)
                .HasMaxLength(100)
                .HasColumnName("nameCarModel");

            entity.HasOne(d => d.CarBrand).WithMany(p => p.CarModels)
                .HasForeignKey(d => d.CarBrandId)
                .HasConstraintName("fk_carBrandId_carBrand");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("PK__country__D32076BCB98AE9D4");

            entity.ToTable("country");

            entity.Property(e => e.CountryId).HasColumnName("countryId");
            entity.Property(e => e.NameCountry)
                .HasMaxLength(100)
                .HasColumnName("nameCountry");
        });

        modelBuilder.Entity<Knot>(entity =>
        {
            entity.HasKey(e => e.KnotId).HasName("PK__knot__435C5A9321481E99");

            entity.ToTable("knot");

            entity.Property(e => e.KnotId).HasColumnName("knotId");
            entity.Property(e => e.NameKnot)
                .HasMaxLength(150)
                .HasColumnName("nameKnot");
            entity.Property(e => e.UnitId).HasColumnName("unitId");

            entity.HasOne(d => d.Unit).WithMany(p => p.Knots)
                .HasForeignKey(d => d.UnitId)
                .HasConstraintName("fk_unitId_unit");
        });

        modelBuilder.Entity<Manufacture>(entity =>
        {
            entity.HasKey(e => e.ManufactureId).HasName("PK__manufact__0CCD8D0F6874E255");

            entity.ToTable("manufacture");

            entity.Property(e => e.ManufactureId).HasColumnName("manufactureId");
            entity.Property(e => e.CountryId).HasColumnName("countryId");
            entity.Property(e => e.NameManufacture)
                .HasMaxLength(200)
                .HasColumnName("nameManufacture");

            entity.HasOne(d => d.Country).WithMany(p => p.Manufactures)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("fk_countryId_country");
        });

        modelBuilder.Entity<PartInterchangeability>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("partInterchangeability");

            entity.Property(e => e.AutoPartId).HasColumnName("autoPartId");
            entity.Property(e => e.InterchangeableDetailId).HasColumnName("interchangeableDetailId");
            entity.Property(e => e.PartInterchangeabilityId)
                .ValueGeneratedOnAdd()
                .HasColumnName("partInterchangeabilityId");

            entity.HasOne(d => d.AutoPart).WithMany()
                .HasForeignKey(d => d.AutoPartId)
                .HasConstraintName("fk_autoPartId_autopart");

            entity.HasOne(d => d.InterchangeableDetail).WithMany()
                .HasForeignKey(d => d.InterchangeableDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_interchangeableDetailId_autopart");
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(e => e.UnitId).HasName("PK__unit__55D7923554E076E0");

            entity.ToTable("unit");

            entity.Property(e => e.UnitId).HasColumnName("unitId");
            entity.Property(e => e.NameUnit)
                .HasMaxLength(150)
                .HasColumnName("nameUnit");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

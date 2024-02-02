using System;
using System.Collections.Generic;
using Infrastructure.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public partial class ProductDataContext : DbContext
{
    public ProductDataContext()
    {
    }

    public ProductDataContext(DbContextOptions<ProductDataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Manufacture> Manufactures { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductInformation> ProductInformations { get; set; }

    public virtual DbSet<ProductPrice> ProductPrices { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-RA22D0F\\MSSQLSERVER02;Initial Catalog=database2;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC079B0B4B1A");

            entity.HasIndex(e => e.CategoryName, "UQ__Categori__8517B2E0A29C4E00").IsUnique();

            entity.Property(e => e.CategoryName).HasMaxLength(50);
        });

        modelBuilder.Entity<Manufacture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Manufact__3214EC07A8F32DB9");

            entity.HasIndex(e => e.ManufactureName, "UQ__Manufact__00DD03CE49585EB3").IsUnique();

            entity.Property(e => e.ManufactureName).HasMaxLength(50);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ArticleNumber).HasName("PK__Products__3C991143120CCCC4");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Products__Catego__3D5E1FD2");

            entity.HasOne(d => d.Manufacture).WithMany(p => p.Products)
                .HasForeignKey(d => d.ManufactureId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Products__Manufa__3E52440B");
        });

        modelBuilder.Entity<ProductInformation>(entity =>
        {
            entity.HasKey(e => e.ArticleNumber).HasName("PK__ProductI__3C991143D1903174");

            entity.ToTable("ProductInformation");

            entity.Property(e => e.Ingress).HasMaxLength(200);
            entity.Property(e => e.ProductTitle).HasMaxLength(200);

            entity.HasOne(d => d.ArticleNumberNavigation).WithOne(p => p.ProductInformation)
                .HasForeignKey<ProductInformation>(d => d.ArticleNumber)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ProductIn__Artic__412EB0B6");
        });

        modelBuilder.Entity<ProductPrice>(entity =>
        {
            entity.HasKey(e => e.ArticleNumber).HasName("PK__ProductP__3C991143C684862B");

            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.ArticleNumberNavigation).WithOne(p => p.ProductPrice)
                .HasForeignKey<ProductPrice>(d => d.ArticleNumber)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ProductPr__Artic__440B1D61");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

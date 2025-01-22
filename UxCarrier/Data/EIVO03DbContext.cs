using Microsoft.EntityFrameworkCore;
using UxCarrier.Models.Entities;

namespace UxCarrier.Data
{
    public class EIVO03DbContext : DbContext
    {

        public EIVO03DbContext(DbContextOptions<EIVO03DbContext> options) : base(options) 
        { 
        }

        public DbSet<InvoiceItem> InvoiceItem { get; set; }
        public DbSet<InvoiceCarrier> InvoiceCarrier { get; set; }
        public DbSet<InvoiceDetails> InvoiceDetails { get; set; }
        public DbSet<InvoiceProduct> InvoiceProduct { get; set; }
        public DbSet<InvoiceProductItem> InvoiceProductItem { get; set; }
        public DbSet<InvoiceAmountType> InvoiceAmountType { get; set; }
        public DbSet<InvoiceSeller> InvoiceSeller { get; set; }
        public DbSet<CurrencyType> CurrencyType { get; set; }
        public DbSet<InvoiceWinningNumber> InvoiceWinningNumber { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InvoiceItem>()
                .HasKey(b => b.InvoiceID);
            modelBuilder.Entity<InvoiceCarrier>().HasKey(b => b.InvoiceID);
            modelBuilder.Entity<InvoiceCarrier>()
                .HasOne(e => e.InvoiceItem)
                .WithOne(e => e.InvoiceCarrier)
                .HasForeignKey<InvoiceItem>(e => e.InvoiceID)
                .IsRequired();
            modelBuilder.Entity<InvoiceSeller>().HasKey(b => b.InvoiceID);
            modelBuilder.Entity<InvoiceSeller>()
                .HasOne(e => e.InvoiceItem)
                .WithOne(e => e.InvoiceSeller)
                .HasForeignKey<InvoiceItem>(e => e.InvoiceID)
                .IsRequired();
            modelBuilder.Entity<InvoiceAmountType>().HasKey(e => e.InvoiceID);
            modelBuilder.Entity<InvoiceAmountType>()
                .HasOne(x=>x.InvoiceItem)
                .WithOne(x=>x.InvoiceAmountType)
                .HasForeignKey<InvoiceItem>(e=>e.InvoiceID);
            modelBuilder.Entity<InvoiceAmountType>().Property(prop => prop.SalesAmount).HasPrecision(18, 5);
            modelBuilder.Entity<InvoiceAmountType>().Property(prop => prop.TaxAmount).HasPrecision(18, 5);
            modelBuilder.Entity<InvoiceAmountType>().Property(prop => prop.TaxRate).HasPrecision(18, 5);
            modelBuilder.Entity<InvoiceAmountType>().Property(prop => prop.TotalAmount).HasPrecision(18, 5);
            modelBuilder.Entity<CurrencyType>().HasKey(e => e.CurrencyID);
            modelBuilder.Entity<InvoiceAmountType>()
                .HasOne(x => x.CurrencyType)
                .WithOne(x => x.InvoiceAmountType)
                .HasForeignKey<CurrencyType>(e => e.CurrencyID);
            modelBuilder.Entity<InvoiceDetails>().HasKey(b => b.InvoiceID);
            modelBuilder.Entity<InvoiceDetails>()
                .HasOne(e => e.InvoiceItem)
                .WithMany(e => e.InvoiceDetails)
                .HasForeignKey(e => e.InvoiceID)
                .IsRequired();
            modelBuilder.Entity<InvoiceProduct>().HasKey(b => b.ProductID);
            modelBuilder.Entity<InvoiceProduct>()
                .HasOne(e => e.InvoiceDetails)
                .WithOne(e => e.InvoiceProduct)
                .HasForeignKey<InvoiceDetails>(b => b.ProductID)
                .IsRequired();
            modelBuilder.Entity<InvoiceProductItem>().HasKey(key => key.ProductID);
            modelBuilder.Entity<InvoiceProductItem>().Property(prop => prop.CostAmount).HasPrecision(18,5);
            modelBuilder.Entity<InvoiceProductItem>().Property(prop => prop.Piece).HasPrecision(18,5);
            modelBuilder.Entity<InvoiceProductItem>().Property(prop => prop.UnitCost).HasPrecision(18,5);
            modelBuilder.Entity<InvoiceProductItem>()
                .HasOne(e => e.InvoiceDetails)
                .WithOne(e => e.InvoiceProductItem)
                .HasForeignKey<InvoiceDetails>(b => b.ProductID)
                .IsRequired();
            modelBuilder.Entity<InvoiceWinningNumber>().HasKey(e => e.InvoiceID);
            modelBuilder.Entity<InvoiceWinningNumber>()
                .HasOne(x => x.InvoiceItem)
                .WithOne(x => x.InvoiceWinningNumber)
                .HasForeignKey<InvoiceItem>(e => e.InvoiceID)
                .IsRequired(false);
            modelBuilder.Entity<InvoiceBuyer>().HasKey(e => e.InvoiceID);
            modelBuilder.Entity<InvoiceBuyer>()
                .HasOne(x => x.InvoiceItem)
                .WithOne(x => x.InvoiceBuyer)
                .HasForeignKey<InvoiceItem>(e => e.InvoiceID)
                .IsRequired(false); ;
        }
    }
}

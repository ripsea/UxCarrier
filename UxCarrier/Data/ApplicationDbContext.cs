using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using UxCarrier.Models.Entities;

namespace UxCarrier.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        }

        public DbSet<EInvoiceBindCard> EInvoiceBindCards { get; set; }
        public DbSet<UxBindCard> UxBindCards { get; set; }
        public DbSet<UxCardEmail> UxCardEmails { get; set; }
        public DbSet<UxCard> UxCard { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UxCard>(c =>
            {
                c.Property(p => p.CreatedDate).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<UxCard>()
                .HasKey(x => x.CardNo);

            modelBuilder.Entity<UxBindCard>(c=>
            { 
                c.Property(p => p.CreatedDate).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
            });

            //modelBuilder.Entity<UxBindCard>()
            //    .HasOne(ux => ux.UxCard)
            //    .WithOne(card => card.UxBindCard)
            //    .HasForeignKey<UxCard>(e => e.CardNo);

            modelBuilder.Entity<UxCardEmail>(c =>
            {
                c.Property(p => p.CreatedDate).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<UxCardEmail>()
                .HasOne<UxCard>(s => s.UxCard)
                .WithMany(g => g.MemberEmails)
                .HasForeignKey(s => s.CardNo);

            modelBuilder.Entity<EInvoiceBindCard>(c =>
            {
                c.Property(p => p.CreatedDate).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
            });

            //modelBuilder.Entity<EInvoiceBindCard>()
            //    .HasOne(ux => ux.UxCard)
            //    .WithOne(card => card.EInvoiceBindCard)
            //    .HasForeignKey<UxCard>(e => e.CardNo);
        }
    }
}

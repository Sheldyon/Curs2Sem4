using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CyberHeaven.Models
{
   public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<PlaceCategory> PlaceCategories { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<Review> Reviews { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["CyberHeavenDB"].ConnectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Place>()
                .HasOne(p => p.Category)
                .WithMany(pc => pc.Places)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Booking>().ToTable("Bookings")
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Place)
                .WithMany()
                .HasForeignKey(b => b.PlaceId);
            modelBuilder.Entity<Review>().ToTable("Reviews")
         .HasOne(r => r.User)
         .WithMany(u => u.Reviews)
         .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<PlaceCategory>().ToTable("PlaceCategories");
            modelBuilder.Entity<Place>().ToTable("Places");
            modelBuilder.Entity<PromoCode>().ToTable("PromoCodes");


            modelBuilder.Entity<PromoCode>().ToTable("PromoCodes");

        }
    }
}
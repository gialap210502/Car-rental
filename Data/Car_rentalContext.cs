using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Car_rental.Models;

namespace Car_rental.Data
{
    public class Car_rentalContext : DbContext
    {
        public Car_rentalContext(DbContextOptions<Car_rentalContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<user>()
                .HasMany(u => u.userRoles)
                .WithOne(c => c.user)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(c => c.userId);

            modelBuilder.Entity<roles>()
                .HasMany(u => u.userRoles)
                .WithOne(c => c.role)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(c => c.roleId);

            modelBuilder.Entity<user>()
                .HasMany(u => u.cars)
                .WithOne(r => r.user)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(c => c.user_id);

            modelBuilder.Entity<user>()
                .HasMany(u => u.bookings)
                .WithOne(r => r.user)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(c => c.userId);

            modelBuilder.Entity<car>()
                .HasMany(u => u.payments)
                .WithOne(r => r.car)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<car>()
                .HasMany(u => u.ratings)
                .WithOne(r => r.car)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(c => c.carId);

            modelBuilder.Entity<category>()
                .HasMany(c => c.cars)
                .WithOne(r => r.category)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(c => c.category_id);

            modelBuilder.Entity<discount>()
                .HasMany(d => d.cars)
                .WithOne(r => r.Discount)
                .HasForeignKey(r => r.discount_id);

            modelBuilder.Entity<user>()
                .HasMany(u => u.ratings)
                .WithOne(r => r.user)
                .HasForeignKey(r => r.userId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<bookings>()
                .HasMany(u => u.payments)
                .WithOne(r => r.booking)
                .HasForeignKey(r => r.booking_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<payment>()
                .HasOne(r => r.booking)
                .WithMany(u => u.payments)
                .HasForeignKey(r => r.booking_id)
                .OnDelete(DeleteBehavior.Restrict);

        }

        public DbSet<Car_rental.Models.car> Car { get; set; } = default!;

        public DbSet<Car_rental.Models.roles> roles { get; set; } = default!;

        public DbSet<Car_rental.Models.userRole> userRole { get; set; } = default!;

        public DbSet<Car_rental.Models.user> user { get; set; } = default!;

        public DbSet<Car_rental.Models.payment> payment { get; set; } = default!;

        public DbSet<Car_rental.Models.bookings> bookings { get; set; } = default!;

        public DbSet<Car_rental.Models.rating> rating { get; set; } = default!;

        public DbSet<Car_rental.Models.discount> discount { get; set; } = default!;

        public DbSet<Car_rental.Models.category> category { get; set; } = default!;

        public DbSet<Car_rental.Models.Images> Images { get; set; }

        public DbSet<Car_rental.Models.Conversation> Conversation { get; set; } = default!;

        public DbSet<Car_rental.Models.Message> Message { get; set; } = default!;
        public DbSet<Car_rental.Models.paymentHistory> paymentHistory { get; set; } = default!;
        
        public DbSet<Car_rental.Models.Participation> Participation { get; set; } = default!;
        

    }
}

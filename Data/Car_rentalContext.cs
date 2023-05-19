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
        public Car_rentalContext (DbContextOptions<Car_rentalContext> options)
            : base(options)
        {
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
    }
}

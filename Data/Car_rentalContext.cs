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

        public DbSet<Car_rental.Models.Car> Car { get; set; } = default!;
    }
}

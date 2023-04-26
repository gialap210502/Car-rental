using System.ComponentModel.DataAnnotations;

namespace Car_rental.Models;

    public class Car
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string? Type { get; set; }
        public double Price { get; set; }
    }

using System.ComponentModel.DataAnnotations;

namespace Car_rental.Models;

public class payment
{
    public int id { get; set; }
    [Required]
    public double amount { get; set; }
    [Required]
    public DateTime paymentDate { get; set; }
    [Required]
    public string? paymentMethod { get; set; }
    public int? status { get; set; }

    public int carId { get; set; }

    public car? car { get; set; }

    public int booking_id { get; set; }

    public bookings? booking { get; set; }

}
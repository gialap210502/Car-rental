using System.ComponentModel.DataAnnotations;

namespace Car_rental.Models;

public class payment
{
    public int id { get; set; }
    public double amount { get; set; }
    public DateTime paymentDate { get; set; }
    public string? paymentMethod { get; set; }
    public int carId { get; set; }
    public car? car { get; set; }
    public int booking_id { get; set; }
    public bookings? booking { get; set; }

}
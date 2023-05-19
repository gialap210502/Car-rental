using System.ComponentModel.DataAnnotations;

namespace Car_rental.Models;

public class bookings
{
    public int id { get; set; }
    [Required]
    public DateTime? startDate { get; set; }
    [Required]
    public DateTime? endDate { get; set; }
    public double? totalAmount { get; set; }
    public int userId { get; set; }
    public user? user { get; set; }
    public ICollection<payment>? payments { get; } = new List<payment>();
}
using System.ComponentModel.DataAnnotations;

namespace Car_rental.Models;

public class bookings
{
    public int id { get; set; }
    [Required]
    [DataType(DataType.Date)]
    public DateTime? startDate { get; set; }
    [Required]
    [DataType(DataType.Date)]
    public DateTime? endDate { get; set; }
    public string TakeCar { get; set; }
    public string CarBack { get; set; }
    public double? totalAmount { get; set; }
    [Required]
    public int userId { get; set; }
    public user? user { get; set; }
    public ICollection<payment>? payments { get; } = new List<payment>();
}
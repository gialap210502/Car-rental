using System.ComponentModel.DataAnnotations;

namespace Car_rental.Models;

public class discount
{
    public int id { get; set; }
    [Required]
    public string? code { get; set; }
    [Required]
    public string? percentage { get; set; }
    [Required]
    [DataType(DataType.Date)]
    public DateTime? startDate { get; set; }
    [Required]
    [DataType(DataType.Date)]
    public DateTime? endDate { get; set; }
    public ICollection<car>? cars { get; set; }

}
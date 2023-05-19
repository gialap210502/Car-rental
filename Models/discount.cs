using System.ComponentModel.DataAnnotations;

namespace Car_rental.Models;

public class discount
{
    public int id { get; set; }
    [Required]
    public string? code { get; set; }
    public string? percentage { get; set; }
    [Required]
    public int startDate { get; set; }
    [Required]
    public int endDate { get; set; }
    public ICollection<car>? cars { get; set; }

}
using System.ComponentModel.DataAnnotations;

namespace Car_rental.Models;

public class category
{
    public int id { get; set; }
    [Required]
    public string? type { get; set; }
    [Required]
    public int Status { get; set; }
    [Required]
    public int Deleted_Status { get; set; }
    public ICollection<car>? cars { get; } = new List<car>();

}
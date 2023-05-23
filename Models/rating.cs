using System.ComponentModel.DataAnnotations;

namespace Car_rental.Models;
public class rating
{
    public int id { get; set; }
    [Required]
    public DateTime? dateRating { get; set; }
    [Required]
    public int Status { get; set; }
    public int Star { get; set; }
    public string? comment { get; set; }
    [Required]
    public int carId { get; set; }

    public car? car { get; set; }
    [Required]
    public int userId { get; set; }

    public user? user { get; set; }
}
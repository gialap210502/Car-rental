using System.ComponentModel.DataAnnotations;

namespace Car_rental.Models;
public class rating
{
    public int id { get; set; }
    public DateTime? dateRating { get; set; }
    public int Status { get; set; }
    public int Star { get; set; }
    public string? comment { get; set; }

    public int carId { get; set; }

    public car? car { get; set; }

    public int userId { get; set; }

    public user? user { get; set; }
}
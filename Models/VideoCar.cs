using System.ComponentModel.DataAnnotations;

namespace Car_rental.Models;

public class VideoCar
{
    public int id { get; set; }
    public string? nameFile { get; set; }
    [Required]
    public int carId { get; set; }
    public car? car { get; set; }
    
}
using System.ComponentModel.DataAnnotations;

namespace Car_rental.Models;

public class paymentHistory
{
    public int id { get; set; }
    [Required]
    public string? Code { get; set; }
    [Required]
    public DateTime timeStamp { get; set; }
    [Required]
    public string Amount { get; set; }
    [Required]
    public string Status { get; set; }
    public int UserID { get; set; }
    public user? user { get; set; }

}
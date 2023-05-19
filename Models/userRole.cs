using System.ComponentModel.DataAnnotations;

namespace Car_rental.Models;

public class userRole
{
    public int id { get; set; }
    [Required]
    public int userId { get; set; }
    public user? user { get; set; }
    [Required]
    public int roleId { get; set; }
    public roles? role { get; set; }


}

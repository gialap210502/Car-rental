using System.ComponentModel.DataAnnotations;

namespace Car_rental.Models;

public class roles
{
    public int id { get; set; }
    [Required]
    public string? role { get; set; }
    public ICollection<userRole> userRoles { get; } = new List<userRole>();
}

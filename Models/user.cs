using System.ComponentModel.DataAnnotations;

namespace Car_rental.Models;
public class user
{
    [Display(Name = "ID")]
    public int id { get; set; }

    [Required]
    [Display(Name = "Name")]
    public string? name { get; set; }

    [Display(Name = "Citizen identification")]
    public string? citizen_identification { get; set; }

    [Display(Name = "Driver license")]
    public string? driver_license { get; set; }

    [Required]
    [Display(Name = "Phone")]
    public string? phone { get; set; }
    [Required]
    [Display(Name = "address")]
    public string? address { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "DOB")]
    public DateTime? dob { get; set; }
    [Required]
    [Display(Name = "Email")]
    public string? email { get; set; }
    [Required]
    [Display(Name = "Password")]
    public string? password { get; set; }
    [Required]
    [Display(Name = "Flag")]
    public int flag { get; set; } = 0;

    public string? image { get; set; }
    public double? coins { get; set; }

    public ICollection<userRole> userRoles { get; } = new List<userRole>();

    public ICollection<rating>? ratings { get; } = new List<rating>();
    public ICollection<bookings>? bookings { get; } = new List<bookings>();

    public ICollection<car>? cars { get; } = new List<car>();
    public ICollection<Message>? Messages { get; } = new List<Message>();
    public ICollection<paymentHistory>? paymentHistory { get; } = new List<paymentHistory>();
    public ICollection<Participation>? Participations { get; } = new List<Participation>();
}
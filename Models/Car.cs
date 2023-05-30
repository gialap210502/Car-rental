using System.ComponentModel.DataAnnotations;

namespace Car_rental.Models;

public class car
{
    public int id { get; set; }
    public string? model { get; set; }
    public string? brand { get; set; }
    public int? seat { get; set; }
    public string? color { get; set; }
    public string? address { get; set; }
    public int? available { get; set; }

    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }
    public string? Type { get; set; }
    public double Price { get; set; }
    public ICollection<payment>? payments { get; } = new List<payment>();
    public ICollection<rating>? ratings { get; } = new List<rating>();
    public ICollection<Images> images { get; } = new List<Images>();
    public int discount_id { get; set; }
    public discount? Discount { get; set; }
    [Required]
    public int user_id { get; set; }
    public user? user { get; set; }
    [Required]
    public int category_id { get; set; }
    public category? category { get; set; }
}

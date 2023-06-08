using System.ComponentModel.DataAnnotations;

namespace Car_rental.Models;

public class car
{
    public int id { get; set; }
    public string? model { get; set; }
    public string? brand { get; set; }
    public int? seat { get; set; }
    public long? Mileage { get; set; }
    public string? Transmission { get; set; }

    public string? color { get; set; }
    public string? address { get; set; }
    public int? available { get; set; }

    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }
    public string? Type { get; set; }
    public double Price { get; set; }
    public bool AirConditioning { get; set; }
    public bool ChildSeat { get; set; }
    public bool GPS { get; set; }
    public bool Luggage { get; set; }
    public bool Music { get; set; }
    public bool SeatBelt { get; set; }
    public bool SleepingBed { get; set; }
    public bool Water { get; set; }
    public bool Bluetooth { get; set; }
    public bool OnboardComputer { get; set; }
    public bool AudioInput { get; set; }
    public bool LongTermTrips { get; set; }
    public bool CarKit { get; set; }
    public bool RemoteCentralLocking { get; set; }
    public bool ClimateControl { get; set; }

    public ICollection<payment>? payments { get; } = new List<payment>();
    public ICollection<rating>? ratings { get; } = new List<rating>();
    public ICollection<Images> images { get; } = new List<Images>();
    public ICollection<VideoCar> videoCars { get; } = new List<VideoCar>();
    public int discount_id { get; set; }
    public discount? Discount { get; set; }
    [Required]
    public int user_id { get; set; }
    public user? user { get; set; }
    [Required]
    public int category_id { get; set; }
    public category? category { get; set; }
}

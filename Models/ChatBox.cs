using System.ComponentModel.DataAnnotations;

namespace Car_rental.Models;
public class ChatBox
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public user User { get; set; } 
    public ICollection<Message> Messages { get; set; }
}

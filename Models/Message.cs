using System.ComponentModel.DataAnnotations;

namespace Car_rental.Models;
public class Message
{
    public int Id { get; set; }
    public int ChatBoxId { get; set; } 
    public ChatBox ChatBox { get; set; }
    public int userID { get; set; } 
    public user User { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
}

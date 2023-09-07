using System.ComponentModel.DataAnnotations;

namespace Car_rental.Models;

public class Conversation
{
    public int ConversationID { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Message>? Messages { get; } = new List<Message>();
    public ICollection<Participation>? Participations { get; } = new List<Participation>();
}
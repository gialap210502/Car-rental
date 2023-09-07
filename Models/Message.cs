using System.ComponentModel.DataAnnotations;

namespace Car_rental.Models;
public class Message
{
    public int MessageID { get; set; }
    public int ConversationID { get; set; }
    public Conversation? Conversation { get; set; }
    public int UserID { get; set; }
    public user? user { get; set; }
    public string Content { get; set; }
    public DateTime SentAt { get; set; }
}

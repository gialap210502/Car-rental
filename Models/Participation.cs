using System.ComponentModel.DataAnnotations;

namespace Car_rental.Models;

public class Participation
{
    public int ParticipationID { get; set; }
    public int UserID { get; set; }
    public user? user { get; set; }
    public int ConversationID { get; set; }
    public Conversation? Conversation { get; set; }
}
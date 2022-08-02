using System.ComponentModel.DataAnnotations;

namespace API.Entities
{

    public class FriendRequest
    {
     public  int Id { get; set; }
    public DateTime DateRequested { get; set; }
     public int RequesterId { get; set; }
    public AppUser Requester { get; set; }
    public AppUser Receiver { get; set; }
    public int Receiverid { get; set; }
    public bool Status {get; set;} = false;
    }
}
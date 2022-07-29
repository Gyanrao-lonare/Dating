namespace API.Entities
{

    public class FriendRequest
    {
     public int Id { get; set; }
    public DateTime DateRequested { get; set; }
    public AppUser Requester { get; set; }
    public AppUser Receiver { get; set; }
    public string Status {get; set;}
    }
}
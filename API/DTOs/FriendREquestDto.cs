namespace API.DTOs
{
    public class FriendRequestDto
    {
        public int Id { get; set; }
        public string userName { get; set; }
        public int Age { get; set; }
        public string knownAs { get; set; }
        public string PhotoUrl { get; set; }
        public string City { get; set; }
        public bool Status { get; set; }
    }
}
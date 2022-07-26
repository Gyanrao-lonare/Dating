namespace API.Entities
{
    public class UserStatus
    {
        public int id { get; set; }
        public string status { get; set; }
        public string color { get; set; }
        public bool isActive { get; set; } = true;
    }
}
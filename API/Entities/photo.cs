using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("photos")]
    public class photo
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public bool IsAproved { get; set; }
        public string PublicId { get; set; }
        public AppUser AppUser { get; set; }
        public int AppUserId {get; set;}
    }
}
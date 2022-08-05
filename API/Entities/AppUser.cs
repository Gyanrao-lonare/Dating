using System;
using API.Extenstions;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public DateTime DateOfBirth { get; set; }
        public String KnownAs { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;

        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Intrests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<photo> Photos { get; set; }

        public ICollection<UserLike> LikedByUsers { get; set; }
        public ICollection<UserLike> LikedUsers { get; set; }

        public ICollection<Message> MessageSent { get; set; }
        public ICollection<Message> MessageRecived { get; set; }
        public ICollection<AppUserRole> UserRoles {get; set;} 
        public ICollection<FriendRequest> RequestedRequests { get; set; }
        public ICollection<FriendRequest> RecevedRequests { get; set; }
        


    }
}
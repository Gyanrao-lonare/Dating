using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void update(AppUser user);
          void UpdatePhotosAsync(photo photo);

        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task <AppUser> GetUserByIdAsync(int id);
        Task <AppUser> GetUserByUsernameAsync (string username);
        Task <PagedList<MemberDto>> GetMembersAsync(UserParms userParms);
        Task <MemberDto> GetMemberAsync (string username); 
        IEnumerable<photo> GetPhotosAsync();
        Task <photo> GetphotoAsync(int id);

}
}
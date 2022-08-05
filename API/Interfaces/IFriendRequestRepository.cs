using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{

    public interface IFriendRequestRepository
    {
        
       
        void SendRequest(FriendRequest friendRequest);
        void DeleteRequest(FriendRequest friendRequest);
           void Update(FriendRequest friendRequest);
        Task<FriendRequest> GetUserFriendRequest(int id);
    Task<FriendRequest>GetFriend (int Receiverid , int RequesterId);
        Task<AppUser> GetUserWithFriendRequest(int userId);
        Task <PagedList<FriendRequestDto>> GetUserFriendRequest(RequestParms requestParms);
        Task<bool> SaveAllAsync();

    }
}
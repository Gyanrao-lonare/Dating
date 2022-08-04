using API.DTOs;
using API.Entities;
using API.Extenstions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class FriendRequestRepository : IFriendRequestRepository
    {
        private readonly DataContext _context;
        public FriendRequestRepository(DataContext context)
        {
            _context = context;
        }

        public void DeleteRequest(FriendRequest friendRequest)
        {
            _context.Freinds.Remove(friendRequest);
        }

        public async Task<FriendRequest> GetFriend(int Receiverid, int RequesterId)
        {
            return _context.Freinds.FirstOrDefault(x => x.Receiverid == Receiverid && x.RequesterId == RequesterId);
        }
        public async Task<FriendRequest> GetUserFriendRequest(int id)
        {
            return await _context.Freinds.FindAsync(id);
        }

        public async Task<PagedList<FriendRequestDto>> GetUserFriendRequest(RequestParms requestParms)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            // var users1 = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var friends = _context.Freinds.AsQueryable();

            if (requestParms.Predicate == "Requests")
            {
                friends = friends.Where(friend => friend.RequesterId == requestParms.UserId && friend.Status == false);
                users = friends.Select(friend => friend.Receiver);
            }

            if (requestParms.Predicate == "Recives")
            {
                friends = friends.Where(friend => friend.Receiverid == requestParms.UserId && friend.Status == false);
                users = friends.Select(friend => friend.Requester);
            }
            
            if (requestParms.Predicate == "Friends")
            {
              friends = friends.Where(friend =>friend.Status == true && (friend.Receiverid == requestParms.UserId || friend.RequesterId == requestParms.UserId));
              users = friends.Select(friend =>friend.Receiverid == requestParms.UserId ? friend.Requester : friend.Receiver);
            //   users = friends.Select(friend => friend.Receiver);
            //   users = friends.Select(friend => friend.Requester);
            }

            if (requestParms.Predicate == "Friends")
            {
               var likedUser = users.Select(user => new FriendRequestDto
                {
                    userName = user.UserName,
                    knownAs = user.KnownAs,
                    Age = user.DateOfBirth.CalculateAge(),
                    // PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                    City = user.City,
                    Id = user.Id,
                    Status = true,
                });
                 return await PagedList<FriendRequestDto>.CreateAsync(likedUser, requestParms.pageNumber, requestParms.PageSize);
            }
            else
            {
             var likedUser = users.Select(user => new FriendRequestDto
                {
                    userName = user.UserName,
                    knownAs = user.KnownAs,
                    Age = user.DateOfBirth.CalculateAge(),
                    PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain && p.IsAproved).Url,
                    City = user.City,
                    Id = user.Id,
                    Status = false
                });
                 return await PagedList<FriendRequestDto>.CreateAsync(likedUser, requestParms.pageNumber, requestParms.PageSize);
            }
           

        }

        public async Task<AppUser> GetUserWithFriendRequest(int userId)
        {
            return await _context.Users.Include(x => x.RecevedRequests)
             .FirstOrDefaultAsync(x => x.Id == userId);
        }
        public void Update(FriendRequest friendRequest)
        {
            _context.Entry(friendRequest).State = EntityState.Modified;
        }
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void SendRequest(FriendRequest friendRequest)
        {
            _context.Freinds.Add(friendRequest);
        }


    }
}